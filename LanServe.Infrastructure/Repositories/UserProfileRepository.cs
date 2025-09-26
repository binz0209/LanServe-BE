using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(IMongoCollection<UserProfile> collection) : base(collection) { }

    public Task<UserProfile?> GetByUserIdAsync(string userId)
        => _collection.Find(p => p.UserId == userId).FirstOrDefaultAsync();
}
