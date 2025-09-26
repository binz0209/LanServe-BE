using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IUserProfileRepository : IGenericRepository<UserProfile>
{
    Task<UserProfile?> GetByUserIdAsync(string userId);
}
