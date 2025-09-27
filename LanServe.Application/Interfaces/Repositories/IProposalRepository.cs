using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IProposalRepository : IGenericRepository<Proposal>
{
    Task<Proposal?> GetByProjectAndFreelancerAsync(string projectId, string freelancerId);
    Task<IEnumerable<Proposal>> GetByProjectAsync(string projectId);
    Task<IEnumerable<Proposal>> GetByFreelancerAsync(string freelancerId);
    Task<IReadOnlyList<Proposal>> GetByProjectIdAsync(string projectId);
}
