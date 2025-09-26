using LanServe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public class ProjectSkillService : IProjectSkillService
    {
        private readonly IProjectSkillRepository _projectSkillRepository;

        public ProjectSkillService(IProjectSkillRepository projectSkillRepository)
        {
            _projectSkillRepository = projectSkillRepository;
        }

        public async Task<ProjectSkill> AddSkillToProjectAsync(ProjectSkill projectSkill)
        {
            await _projectSkillRepository.AddAsync(projectSkill);
            return projectSkill;
        }

        public async Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId)
            => await _projectSkillRepository.GetByProjectIdAsync(projectId);

        public async Task DeleteAsync(string id) => await _projectSkillRepository.DeleteAsync(id);
    }

}
