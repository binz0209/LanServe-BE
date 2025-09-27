using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services;

public interface IProjectSkillService
{
    Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId);
    Task<IEnumerable<ProjectSkill>> GetBySkillIdAsync(string skillId);
    Task<ProjectSkill?> GetByIdAsync(string id);
    Task<ProjectSkill> CreateAsync(ProjectSkill entity);
    Task<bool> DeleteAsync(string id);
}
