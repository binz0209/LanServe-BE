using LanServe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> CreateAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
            return project;
        }

        public async Task<Project?> GetByIdAsync(string id) => await _projectRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Project>> GetAllAsync() => await _projectRepository.GetAllAsync();

        public async Task UpdateAsync(Project project) => await _projectRepository.UpdateAsync(project);

        public async Task DeleteAsync(string id) => await _projectRepository.DeleteAsync(id);
    }
}
