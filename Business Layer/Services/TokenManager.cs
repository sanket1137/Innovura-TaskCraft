using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.IServices;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Business_Layer.Services
{
    public class TokenManager: ITokenManager
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public TokenManager(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        //public async Task<RefreshToken> GetTokenByUserIdAsync(string id, string AccessToken)
        //{
        //    return await _refreshTokenRepository.GetTokenByUserIdAsync(id, AccessToken);
        //}

        public async Task<RefreshToken> GetTokenByUserIdAsync(int id, string AccessToken)
        {
            return await _refreshTokenRepository.GetTokenByUserIdAsync(id, AccessToken);
        }
        public async Task<string> CreateTokenAsync(RefreshToken task)
        {
            return await _refreshTokenRepository.CreateTokenAsync(task);
        }

        public async Task<int> UpdateTokenAsync(RefreshToken task)
        {
            return await _refreshTokenRepository.UpdateTokenAsync(task);
        }


    }
}
