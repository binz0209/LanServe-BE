using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface IProjectSkillRepository : IGenericRepository<ProjectSkill>
{
    Task<IEnumerable<ProjectSkill>> GetByProjectAsync(string projectId);
}
