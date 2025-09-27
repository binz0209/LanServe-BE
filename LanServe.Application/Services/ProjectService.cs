using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;

namespace LanServe.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repo;

    public ProjectService(IProjectRepository repo)
    {
        _repo = repo;
    }

    public Task<Project?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public Task<IEnumerable<Project>> GetByOwnerIdAsync(string ownerId)
        => _repo.GetByOwnerIdAsync(ownerId);

    public Task<IEnumerable<Project>> GetOpenProjectsAsync()
        => _repo.GetOpenProjectsAsync();

    public Task<Project> CreateAsync(Project entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.Status = "Open";
        return _repo.InsertAsync(entity);
    }

    public async Task<bool> UpdateAsync(string id, Project entity)
    {
        entity.Id = id;
        return await _repo.UpdateAsync(entity);
    }

    public Task<bool> DeleteAsync(string id)
        => _repo.DeleteAsync(id);
}
