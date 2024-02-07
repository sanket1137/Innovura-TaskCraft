using Business_Layer.IServices;
using DataAccess.Entities;
using Innovura_TaskCraft.IServices;
using Innovura_TaskCraft.Models;
using Innovura_TaskCraft.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Innovura_TaskCraft.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ITokenManager _tokenManager;
        private readonly Jwt _jwtSettings;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public AuthController(IUserManager userManager, IOptions<Jwt> options, IRefreshTokenGenerator refreshTokenGenerator, ITokenManager tokenManager)
        {
            _userManager = userManager;
            _jwtSettings = options.Value;
            _refreshTokenGenerator= refreshTokenGenerator;
            _tokenManager= tokenManager;
        }

        [NonAction]
        public async Task<TokenResponse> Authenticate(string username, Claim[] claims)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var tokenhandler = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                 signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)

                );
            tokenResponse.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenhandler);
            tokenResponse.RefreshToken = await GenerateToken(username);

            return tokenResponse;
        }
        [NonAction]
        public async Task<string> GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string refreshToken = Convert.ToBase64String(randomnumber);
                User _user = await _userManager.GetUserByEmailAsync(username);
                var token =await _tokenManager.GetTokenByUserIdAsync(_user.Id, refreshToken);
                if (token != null)
                {
                    token.Refresh_Token = refreshToken;
                    //context.SaveChanges();
                }
                else
                {
                    RefreshToken tblRefreshtoken = new RefreshToken()
                    {
                        UserId = _user.Id,
                        TokenId = new Random().Next().ToString(),
                        Refresh_Token = refreshToken,
                        IsActive = true
                    };
                }

                return refreshToken;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                TokenResponse _token = new TokenResponse();
                if (string.IsNullOrEmpty(user.UserEmailId) ||
                string.IsNullOrEmpty(user.Password))
                    return BadRequest("Username and/or Password not specified");

                var isUserCredValid = await _userManager.GetUserByEmailAndPasswordAsync(user.UserEmailId, user.Password);
                if (isUserCredValid)
                {
                    //var jwtSecurityToken = await _userManager.GenerateJwtToken(user);
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(
                        new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.UserEmailId),
                            }
                        ),
                        Expires = DateTime.Now.AddMinutes(15),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenhandler.CreateToken(tokenDescriptor);
                    string finalToken = tokenhandler.WriteToken(token);
                    _token.JWTToken = finalToken;
                    _token.RefreshToken =await GenerateToken(user.UserEmailId);
                    return Ok(finalToken);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest
                ("An error occurred in generating the token");
            }
            return Unauthorized();

        }
        
        [Route("Refresh")]
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] TokenResponse token)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principle = tokenhandler.ValidateToken(token.JWTToken, new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out securityToken);
            var _token = securityToken as JwtSecurityToken;
            if (_token != null && !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }
            var _user = await _userManager.GetUserByEmailAsync(principle.Identity.Name);

            var dbToken = _tokenManager.GetTokenByUserIdAsync(_user.Id, token.RefreshToken);
            if (dbToken == null)
            {
                return Unauthorized();
            }

            TokenResponse newToken = await Authenticate(principle.Identity.Name, principle.Claims.ToArray());
            return Ok(newToken);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            User tempUser = await _userManager.GetUserByEmailAsync(user.UserEmailId);
            if (tempUser == null)
            {
                var createdUser = _userManager.AddUserAsync(user);
                if (createdUser != null)
                {
                    //var token = _userManager.GenerateJwtToken(user);
                    var tokenhandler = new JwtSecurityTokenHandler();
                    var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(
                        new Claim[]
                            {
                                new Claim(ClaimTypes.Name, user.UserEmailId),
                            }
                        ),
                        Expires = DateTime.Now.AddMinutes(15),
                        SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
                    };
                    var token = tokenhandler.CreateToken(tokenDescriptor);
                    string finalToken = tokenhandler.WriteToken(token);
                    return Ok(finalToken);

                }
                else
                {
                    return BadRequest("Failed to add user.");
                }

            }
            else
            {
                ModelState.AddModelError("Error", "User with this email already exists.");
                return Conflict(); //409
            }

        }
    }
}
