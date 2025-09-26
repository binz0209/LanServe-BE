using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository(IMongoCollection<Project> collection) : base(collection) { }

    public async Task<IEnumerable<Project>> GetByOwnerAsync(string ownerId, int skip = 0, int limit = 20)
    {
        return await _collection
            .Find(p => p.OwnerId == ownerId)
            .SortByDescending(p => p.CreatedAt)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
    }
}
