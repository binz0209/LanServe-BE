using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IProjectSkillRepository : IGenericRepository<ProjectSkill>
{
    Task<IEnumerable<ProjectSkill>> GetByProjectAsync(string projectId);
}
