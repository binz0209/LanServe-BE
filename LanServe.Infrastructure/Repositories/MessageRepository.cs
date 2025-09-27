using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(IMongoCollection<Message> collection) : base(collection) { }

        // Giữ nguyên API hiện tại bạn đang dùng
        public async Task<IEnumerable<Message>> GetConversationAsync(string conversationKey, int skip = 0, int limit = 50)
        {
            var cursor = await _collection.Find(m => m.ConversationKey == conversationKey)
                                          .SortBy(m => m.CreatedAt)
                                          .Skip(skip)
                                          .Limit(limit)
                                          .ToListAsync();
            return cursor.AsEnumerable();
        }

        public async Task<int> CountUnreadAsync(string userId)
        {
            var count = await _collection.CountDocumentsAsync(m => m.ReceiverId == userId && !m.IsRead);
            return (int)count;
        }

        // >>>>>>> NEW: implement method đúng chữ ký mà interface đang yêu cầu
        // Map 1-1 với ConversationKey (theo domain của bạn)
        public async Task<IEnumerable<Message>> GetByConversationIdAsync(string conversationId)
        {
            var cursor = await _collection.Find(m => m.ConversationKey == conversationId)
                                          .SortBy(m => m.CreatedAt)
                                          .ToListAsync();
            return cursor.AsEnumerable();
        }
    }
}
