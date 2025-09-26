using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface IUserProfileRepository : IGenericRepository<UserProfile>
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
}
