using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface IProjectRepository : IGenericRepository<Project>
{
    Task<IEnumerable<Project>> GetByOwnerAsync(string ownerId, int skip = 0, int limit = 20);
}
