using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.IServices
{
    public interface ITokenManager
    {
        Task<RefreshToken> GetTokenByUserIdAsync(int id, string token);
        Task<string> CreateTokenAsync(RefreshToken task);
        Task<int> UpdateTokenAsync(RefreshToken task);
    }
}
