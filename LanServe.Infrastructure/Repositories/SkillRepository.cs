using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(IMongoCollection<Skill> collection) : base(collection) { }

    public async Task<IEnumerable<Skill>> GetByCategoryAsync(string categoryId)
        => await _collection.Find(s => s.CategoryId == categoryId).ToListAsync();
}
