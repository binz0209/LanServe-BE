using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories
{
    public class ProjectSkillRepository : GenericRepository<ProjectSkill>, IProjectSkillRepository
    {
        public ProjectSkillRepository(IMongoCollection<ProjectSkill> collection) : base(collection) { }

        public async Task<IEnumerable<ProjectSkill>> GetByProjectAsync(string projectId)
        {
            var list = await _collection.Find(ps => ps.ProjectId == projectId).ToListAsync();
            return list.AsEnumerable();
        }

        public async Task<IReadOnlyList<ProjectSkill>> GetByProjectIdAsync(string projectId)
        {
            var list = await _collection.Find(ps => ps.ProjectId == projectId).ToListAsync();
            return list;
        }
    }
}
