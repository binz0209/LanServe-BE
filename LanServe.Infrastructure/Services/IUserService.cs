using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user, string password);
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User?> GetByIdAsync(string id);
        Task<IEnumerable<User>> GetAllAsync();
    }

}
