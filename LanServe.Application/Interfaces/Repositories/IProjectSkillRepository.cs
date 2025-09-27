using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IProjectSkillRepository
{
    Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId);
    Task<IEnumerable<ProjectSkill>> GetBySkillIdAsync(string skillId);
    Task<ProjectSkill?> GetByIdAsync(string id);
    Task<ProjectSkill> InsertAsync(ProjectSkill entity);
    Task<bool> DeleteAsync(string id);
}
