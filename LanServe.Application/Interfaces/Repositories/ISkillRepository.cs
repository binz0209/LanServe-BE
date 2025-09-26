using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface ISkillRepository : IGenericRepository<Skill>
{
    Task<IEnumerable<Skill>> GetByCategoryAsync(string categoryId);
}
