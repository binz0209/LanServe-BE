using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class ContractRepository : GenericRepository<Contract>, IContractRepository
{
    public ContractRepository(IMongoCollection<Contract> collection) : base(collection) { }

    public Task<Contract?> GetByProjectAsync(string projectId)
        => _collection.Find(c => c.ProjectId == projectId).FirstOrDefaultAsync();

    public async Task<IEnumerable<Contract>> GetActiveByUserAsync(string userId, int take = 20)
    {
        var contracts = await _collection.Find(c => (c.ClientId == userId || c.FreelancerId == userId) && c.Status == "Active")
                                         .Limit(take)
                                         .ToListAsync();
        return contracts;
    }
}
