using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.IServices
{
    public interface IUserManager
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<int> AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task<JwtSecurityToken> GenerateJwtToken(User user);
        Task DeleteUserAsync(string userId);
        Task<bool> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}
