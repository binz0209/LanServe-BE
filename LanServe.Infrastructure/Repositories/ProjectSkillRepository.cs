using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class ProjectSkillRepository : GenericRepository<ProjectSkill>, IProjectSkillRepository
{
    public ProjectSkillRepository(IMongoCollection<ProjectSkill> collection) : base(collection) { }

    public async Task<IEnumerable<ProjectSkill>> GetByProjectAsync(string projectId)
    {
        var result = await _collection.Find(ps => ps.ProjectId == projectId).ToListAsync();
        return result.AsEnumerable();
    }
}
