using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;

namespace LanServe.Application.Services;

public class ProjectSkillService : IProjectSkillService
{
    private readonly IProjectSkillRepository _repo;

    public ProjectSkillService(IProjectSkillRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId)
        => _repo.GetByProjectIdAsync(projectId);

    public Task<IEnumerable<ProjectSkill>> GetBySkillIdAsync(string skillId)
        => _repo.GetBySkillIdAsync(skillId);

    public Task<ProjectSkill?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public Task<ProjectSkill> CreateAsync(ProjectSkill entity)
        => _repo.InsertAsync(entity);

    public Task<bool> DeleteAsync(string id)
        => _repo.DeleteAsync(id);
}
