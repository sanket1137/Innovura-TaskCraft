using Business_Layer.IServices;
using DataAccess.Data;
using DataAccess.Entities;
using Innovura_TaskCraft.IServices;
using Innovura_TaskCraft.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.Services
{
    public class RefreshTokenGenerator:IRefreshTokenGenerator
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenManager _tokenManager;
        private readonly IUserManager _userManager;
        private readonly Jwt _jwtSettings;
        public RefreshTokenGenerator(ITokenManager tokenManager, IUserManager userManager ,ApplicationDbContext applicationDb, Jwt jwtSettings)
        {
            _tokenManager = tokenManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _dbContext = applicationDb;
        }
        //public async Task<string> RefreshToken(string userID) {
        //    var randomnumber = new byte[32];
        //    using(var randomnumbergenerator= RandomNumberGenerator.Create())
        //    {
        //        randomnumbergenerator.GetBytes(randomnumber);
        //        string refreshToken = Convert.ToBase64String(randomnumber);

        //        var token = await _tokenManager.GetTokenByUserIdAsync(userID,refreshToken);

        //        return token.Refresh_Token;
        //    }
        //}
        public async Task<string> GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string refreshToken = Convert.ToBase64String(randomnumber);
                User _user = await _userManager.GetUserByEmailAsync(username);
                var token = _tokenManager.GetTokenByUserIdAsync(_user.Id, refreshToken);
                if (token != null)
                {
                    token.Result.Refresh_Token= refreshToken;
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
        //public async Task<TokenResponse> Authenticate(string username, Claim[] claims)
        //{
        //    TokenResponse newToken = new TokenResponse();
        //    var secretKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        //    var jwtSecurityToken = new JwtSecurityToken(
        //        claims: new List<Claim>(),
        //        expires: DateTime.Now.AddMinutes(10),
        //        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
        //    );
        //    newToken.JWTToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        //    newToken.RefreshToken = await RefreshToken(username);
        //    return newToken;
        //}
    }
}
