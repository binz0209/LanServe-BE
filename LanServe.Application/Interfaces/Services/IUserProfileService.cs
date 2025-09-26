using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services
{
    public interface IUserProfileService
    {
        Task<UserProfile> CreateOrUpdateAsync(UserProfile profile);
        Task<UserProfile?> GetByUserIdAsync(string userId);
    }
}
