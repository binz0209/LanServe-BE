using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateOrUpdateAsync(UserProfile profile);
        Task<UserProfile?> GetByUserIdAsync(string userId);
    }
}
