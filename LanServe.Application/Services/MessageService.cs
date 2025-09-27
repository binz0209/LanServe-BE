using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;

namespace LanServe.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repo;

    public MessageService(IMessageRepository repo)
    {
        _repo = repo;
    }

    public Task<IEnumerable<Message>> GetByConversationAsync(string conversationKey)
        => _repo.GetByConversationAsync(conversationKey);

    public Task<IEnumerable<Message>> GetByProjectAsync(string projectId)
        => _repo.GetByProjectAsync(projectId);

    public Task<Message?> GetByIdAsync(string id)
        => _repo.GetByIdAsync(id);

    public Task<Message> SendAsync(Message entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.IsRead = false;
        return _repo.InsertAsync(entity);
    }

    public Task<bool> MarkAsReadAsync(string id)
        => _repo.MarkAsReadAsync(id);
}
