using Azure.Core;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RefreshTokenRepository: IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {

            _dbContext = dbContext;

        }

        //public async Task<RefreshToken> GetTokenByIdAsync(string id)
        //{
        //    return await _dbContext.Token.Include(t => t.TokenId).FirstOrDefaultAsync(t => t.TokenId == id);
        //}
        public async Task<RefreshToken> GetTokenByUserIdAsync(int id,string RefreshToken)
        {
            RefreshToken refToken= await _dbContext.Token
        .Where(rt => rt.UserId == id)
        .FirstOrDefaultAsync();

            var _user= _dbContext.User.FindAsync(id);
            if (refToken != null)
            {
                refToken
                    .Refresh_Token = RefreshToken;
                _dbContext.SaveChanges();
                return refToken;    
            }
            else
            {
                RefreshToken refTtokenoken = new RefreshToken()
                {
                    UserId = _user.Result.Id,
                    TokenId = new Random().Next().ToString(),
                    Refresh_Token = RefreshToken,
                    IsActive = true
                };


                _dbContext.Token.Add(refTtokenoken);
                await _dbContext.SaveChangesAsync();

                return refTtokenoken;
            }

        }
        public async Task<RefreshToken> GetTokenByUserId(int id, string RefreshToken)
        {
            RefreshToken _refToken = _dbContext.Token.Where(rt => rt.UserId == id).FirstOrDefault();
            var _user= await _dbContext.User.FindAsync(id);
            if (_refToken != null)
            {
                _refToken
                    .Refresh_Token = RefreshToken;
                _dbContext.SaveChanges();
                return _refToken;
            }
            else
            {
                RefreshToken refTtokenoken = new RefreshToken()
                {
                    UserId = _refToken.UserId,
                    Refresh_Token = RefreshToken,
                    IsActive = true
                };


                _dbContext.Token.Add(refTtokenoken);
                 _dbContext.SaveChangesAsync();

                return refTtokenoken;
            }

        }
        public async Task<string> CreateTokenAsync(RefreshToken token)
        {
            _dbContext.Token.Add(token);
            await _dbContext.SaveChangesAsync();
            return token.TokenId;
        }
        public async Task<int> UpdateTokenAsync(RefreshToken token)
        {
            _dbContext.Token.Update(token);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> DeleteTokensync(int id)
        {
            var tokens = await _dbContext.Token.FindAsync(id);

            if (tokens != null)
            {
                 _dbContext.Token.Remove(tokens);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
