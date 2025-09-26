using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services
{
    public interface IProjectSkillService
    {
        Task<ProjectSkill> AddSkillToProjectAsync(ProjectSkill projectSkill);
        Task<IEnumerable<ProjectSkill>> GetByProjectIdAsync(string projectId);
        Task DeleteAsync(string id);
    }
}
