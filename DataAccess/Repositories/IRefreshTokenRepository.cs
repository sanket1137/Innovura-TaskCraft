using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetTokenByUserIdAsync(int id,string AccessToken);
        Task<string> CreateTokenAsync(RefreshToken task);
        Task<int> UpdateTokenAsync(RefreshToken task);
    }
}
