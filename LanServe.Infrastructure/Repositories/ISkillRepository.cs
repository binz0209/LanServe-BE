using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface ISkillRepository : IGenericRepository<Skill>
{
    Task<IEnumerable<Skill>> GetByCategoryAsync(string categoryId);
}
