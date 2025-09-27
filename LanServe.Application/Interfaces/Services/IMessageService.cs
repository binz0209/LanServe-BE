using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Services;

public interface IMessageService
{
    Task<IEnumerable<Message>> GetByConversationAsync(string conversationKey);
    Task<IEnumerable<Message>> GetByProjectAsync(string projectId);
    Task<Message?> GetByIdAsync(string id);
    Task<Message> SendAsync(Message entity);
    Task<bool> MarkAsReadAsync(string id);
}
