using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}
