using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories
{
    public class ProposalRepository : GenericRepository<Proposal>, IProposalRepository
    {
        public ProposalRepository(IMongoCollection<Proposal> collection) : base(collection) { }

        // Lấy proposal theo project + freelancer
        public Task<Proposal?> GetByProjectAndFreelancerAsync(string projectId, string freelancerId)
            => _collection.Find(p => p.ProjectId == projectId && p.FreelancerId == freelancerId)
                          .FirstOrDefaultAsync();

        // Đang có: GetByProjectAsync
        public async Task<IEnumerable<Proposal>> GetByProjectAsync(string projectId)
            => await _collection.Find(p => p.ProjectId == projectId).ToListAsync();

        // chữ ký interface yêu cầu
        public async Task<IReadOnlyList<Proposal>> GetByProjectIdAsync(string projectId)
        {
            var list = await _collection.Find(p => p.ProjectId == projectId).ToListAsync();
            return list;
        }

        // Lấy proposal theo freelancer
        public async Task<IEnumerable<Proposal>> GetByFreelancerAsync(string freelancerId)
            => await _collection.Find(p => p.FreelancerId == freelancerId).ToListAsync();
    }
}
