using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(string id);
    Task<IEnumerable<Project>> GetByOwnerIdAsync(string ownerId);
    Task<IEnumerable<Project>> GetOpenProjectsAsync();
    Task<Project> InsertAsync(Project entity);
    Task<bool> UpdateAsync(Project entity);
    Task<bool> DeleteAsync(string id);
}
