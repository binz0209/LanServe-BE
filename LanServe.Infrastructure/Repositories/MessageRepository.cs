using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(IMongoCollection<Message> collection) : base(collection) { }

    public Task<IEnumerable<Message>> GetConversationAsync(string conversationKey, int skip = 0, int limit = 50)
       => _collection.Find(m => m.ConversationKey == conversationKey)
                     .SortBy(m => m.CreatedAt)
                     .Skip(skip).Limit(limit).ToListAsync()
                     .ContinueWith(task => task.Result.AsEnumerable());

    public async Task<int> CountUnreadAsync(string userId)
        => (int)await _collection.CountDocumentsAsync(m => m.ReceiverId == userId && !m.IsRead);
}
