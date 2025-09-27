using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services;

public interface IProjectService
{
    Task<Project?> GetByIdAsync(string id);
    Task<IEnumerable<Project>> GetByOwnerIdAsync(string ownerId);
    Task<IEnumerable<Project>> GetOpenProjectsAsync();
    Task<Project> CreateAsync(Project entity);
    Task<bool> UpdateAsync(string id, Project entity);
    Task<bool> DeleteAsync(string id);
}
