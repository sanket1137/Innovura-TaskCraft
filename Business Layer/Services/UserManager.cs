using Business_Layer.IServices;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Services
{
    public class UserManager: IUserManager
    {
        private readonly IUserRepository _userRepository;
        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userRepository.DeleteUserAsync(userId);
        }
        public async Task<bool> GetUserByEmailAndPasswordAsync(string emailId, string password) => await _userRepository.GetUserByEmailAndPasswordAsync(emailId, password);
    }
}
