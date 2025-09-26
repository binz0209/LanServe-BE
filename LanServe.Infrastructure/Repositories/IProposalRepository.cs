using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface IProposalRepository : IGenericRepository<Proposal>
{
    Task<Proposal?> GetByProjectAndFreelancerAsync(string projectId, string freelancerId);
    Task<IEnumerable<Proposal>> GetByProjectAsync(string projectId);
    Task<IEnumerable<Proposal>> GetByFreelancerAsync(string freelancerId);
}
