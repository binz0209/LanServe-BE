using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfile> CreateOrUpdateAsync(UserProfile profile)
        {
            var existing = await _userProfileRepository.GetByUserIdAsync(profile.UserId);
            if (existing == null)
            {
                await _userProfileRepository.AddAsync(profile);
            }
            else
            {
                profile.Id = existing.Id;
                await _userProfileRepository.UpdateAsync(profile);
            }
            return profile;
        }

        public async Task<UserProfile?> GetByUserIdAsync(string userId)
            => await _userProfileRepository.GetByUserIdAsync(userId);
    }
}
