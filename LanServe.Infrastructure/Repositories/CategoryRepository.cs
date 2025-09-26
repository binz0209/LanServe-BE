using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IMongoCollection<Category> collection) : base(collection) { }

    public Task<Category?> GetByNameAsync(string name)
        => _collection.Find(x => x.Name == name).FirstOrDefaultAsync();
}
