using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() => await _categoryRepository.GetAllAsync();

        public async Task<Category?> GetByIdAsync(string id) => await _categoryRepository.GetByIdAsync(id);

        public async Task DeleteAsync(string id) => await _categoryRepository.DeleteAsync(id);
    }

}
