using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using BCrypt.Net;

namespace LanServe.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public Task<User?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public Task<User?> GetByEmailAsync(string email)
        => _repo.GetByEmailAsync(email);

    public async Task<User> RegisterAsync(string fullName, string email, string password, string role = "User")
    {
        var existing = await _repo.GetByEmailAsync(email);
        if (existing != null) throw new Exception("Email already exists");

        var user = new User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };

        return await _repo.InsertAsync(user);
    }

    public async Task<User?> ValidateUserAsync(string email, string password)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null) return null;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
    }

    public async Task<bool> UpdateAsync(string id, User user)
    {
        user.Id = id;
        return await _repo.UpdateAsync(user);
    }

    public Task<bool> DeleteAsync(string id)
        => _repo.DeleteAsync(id);
}
