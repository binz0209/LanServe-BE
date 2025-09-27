using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using LanServe.Application.Interfaces.Repositories;
using BCrypt.Net;

namespace LanServe.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> RegisterAsync(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

        public async Task<User?> GetByIdAsync(string id) => await _userRepository.GetByIdAsync(id);

        public async Task<IEnumerable<User>> GetAllAsync() => await _userRepository.GetAllAsync();
    }
}
