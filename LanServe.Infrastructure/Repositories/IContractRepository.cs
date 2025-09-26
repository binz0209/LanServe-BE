using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface IContractRepository : IGenericRepository<Contract>
{
    Task<Contract?> GetByProjectAsync(string projectId);
    Task<IEnumerable<Contract>> GetActiveByUserAsync(string userId, int take = 20);
}
