using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IContractRepository : IGenericRepository<Contract>
{
    Task<Contract?> GetByProjectAsync(string projectId);
    Task<IEnumerable<Contract>> GetActiveByUserAsync(string userId, int take = 20);
}
