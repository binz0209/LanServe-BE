using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(IMongoCollection<User> collection) : base(collection) { }

    public Task<User?> GetByEmailAsync(string email)
        => _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
}
