using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
}
