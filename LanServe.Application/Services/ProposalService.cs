using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;

namespace LanServe.Application.Services;

public class ProposalService : IProposalService
{
    private readonly IProposalRepository _repo;

    public ProposalService(IProposalRepository repo)
    {
        _repo = repo;
    }

    public Task<Proposal?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<Proposal>> GetByProjectIdAsync(string projectId)
        => _repo.GetByProjectIdAsync(projectId);

    public Task<IEnumerable<Proposal>> GetByFreelancerIdAsync(string freelancerId)
        => _repo.GetByFreelancerIdAsync(freelancerId);

    public Task<Proposal> CreateAsync(Proposal entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = "Pending";
        return _repo.InsertAsync(entity);
    }

    public Task<bool> UpdateStatusAsync(string id, string status)
        => _repo.UpdateStatusAsync(id, status);

    public Task<bool> DeleteAsync(string id)
        => _repo.DeleteAsync(id);
}
