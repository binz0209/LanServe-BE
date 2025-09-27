using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class ProjectSkillRepository : IProjectSkillRepository
{
    private readonly IMongoCollection<ProjectSkill> _collection;

    public ProjectSkillRepository(IMongoCollection<ProjectSkill> collection)
    {
        _collection = collection;
    }

    public async Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId)
        => await _collection.Find(x => x.ProjectId == projectId).ToListAsync();

    public async Task<IEnumerable<ProjectSkill>> GetBySkillIdAsync(string skillId)
        => await _collection.Find(x => x.SkillId == skillId).ToListAsync();

    public async Task<ProjectSkill?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<ProjectSkill> InsertAsync(ProjectSkill entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
