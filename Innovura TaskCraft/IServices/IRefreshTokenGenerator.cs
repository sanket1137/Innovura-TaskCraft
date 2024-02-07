using Innovura_TaskCraft.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Innovura_TaskCraft.IServices
{
    public interface IRefreshTokenGenerator
    {
        //Task<string> RefreshToken(string refreshToken);
        Task<string> GenerateToken(string username);
    }
}
