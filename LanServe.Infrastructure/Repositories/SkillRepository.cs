using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly IMongoCollection<Skill> _collection;

    public SkillRepository(IMongoCollection<Skill> collection)
    {
        _collection = collection;
    }

    public async Task<IEnumerable<Skill>> GetAllAsync()
        => await _collection.Find(_ => true).ToListAsync();

    public async Task<IEnumerable<Skill>> GetByCategoryIdAsync(string categoryId)
        => await _collection.Find(x => x.CategoryId == categoryId).ToListAsync();

    public async Task<Skill?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Skill> InsertAsync(Skill entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> UpdateAsync(Skill entity)
    {
        var result = await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}
