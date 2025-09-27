using LanServe.Domain.Entities;

namespace LanServe.Application.Interfaces.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetConversationAsync(string conversationKey, int skip = 0, int limit = 50);
        Task<int> CountUnreadAsync(string userId);

        Task<IEnumerable<Message>> GetByConversationIdAsync(string conversationId);
    }
}
