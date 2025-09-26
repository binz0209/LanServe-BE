using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories;

public class ProposalRepository : GenericRepository<Proposal>, IProposalRepository
{
    public ProposalRepository(IMongoCollection<Proposal> collection) : base(collection) { }

    public Task<Proposal?> GetByProjectAndFreelancerAsync(string projectId, string freelancerId)
        => _collection.Find(p => p.ProjectId == projectId && p.FreelancerId == freelancerId)
                      .FirstOrDefaultAsync();

    public async Task<IEnumerable<Proposal>> GetByProjectAsync(string projectId)
        => await _collection.Find(p => p.ProjectId == projectId).ToListAsync();

    public async Task<IEnumerable<Proposal>> GetByFreelancerAsync(string freelancerId)
        => await _collection.Find(p => p.FreelancerId == freelancerId).ToListAsync();
}
