using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext DbContext)
        {
            _dbContext = DbContext;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dbContext.User.FindAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.UserEmailId == email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.User.ToListAsync();
        }
       
        public async Task<int> AddUserAsync(User user)
        {
            _dbContext.User.Add(user);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.User.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await _dbContext.User.FindAsync(userId);
            if (user != null)
            {
                _dbContext.User.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> GetUserByEmailAndPasswordAsync(string emailId, string password)
        {
            var user= await _dbContext.User.Where(u => u.UserEmailId == emailId && u.Password == password)
            .FirstOrDefaultAsync();
            if (user!= null)
            {
                return true;
            }
            return false;
        }

    }
}
