using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface IProjectSkillService
    {
        Task<ProjectSkill> AddSkillToProjectAsync(ProjectSkill projectSkill);
        Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId);
        Task DeleteAsync(string id);
    }
}
