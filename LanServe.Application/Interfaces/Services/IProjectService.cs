using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(Project project);
        Task<Project?> GetByIdAsync(string id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task UpdateAsync(Project project);
        Task DeleteAsync(string id);
    }
}
