using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<IEnumerable<Message>> GetByConversationAsync(string conversationKey);
    Task<IEnumerable<Message>> GetByProjectAsync(string projectId);
    Task<Message?> GetByIdAsync(string id);
    Task<Message> InsertAsync(Message entity);
    Task<bool> MarkAsReadAsync(string id);
}
