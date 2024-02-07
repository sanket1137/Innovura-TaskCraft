using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtMiddleware> _logger;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    public IConfiguration _configuration { get; }

    public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, IConfiguration _configuration)
    {
        _next = next;
        _logger = logger;
        _secretKey = _configuration["Jwt:SecretKey"];
       
    }

    public async Task Invoke(HttpContext context)
    {
        // Check if the request contains a valid JWT token
        if (context.Request.Headers.TryGetValue("Authorization", out var tokenString))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                // Token validation parameters
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                // Validate token
                var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out _);

                // Attach the principal to the context
                context.User = principal;

                await _next(context);
            }
            catch (SecurityTokenValidationException ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
        else
        {
            // If no token is present, return Unauthorized status
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
