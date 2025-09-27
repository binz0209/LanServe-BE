using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IMongoCollection<Message> _collection;

    public MessageRepository(IMongoCollection<Message> collection)
    {
        _collection = collection;
    }

    public async Task<IEnumerable<Message>> GetByConversationAsync(string conversationKey)
        => await _collection.Find(x => x.ConversationKey == conversationKey)
                            .SortBy(x => x.CreatedAt)
                            .ToListAsync();

    public async Task<IEnumerable<Message>> GetByProjectAsync(string projectId)
        => await _collection.Find(x => x.ProjectId == projectId)
                            .SortBy(x => x.CreatedAt)
                            .ToListAsync();

    public async Task<Message?> GetByIdAsync(string id)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Message> InsertAsync(Message entity)
    {
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task<bool> MarkAsReadAsync(string id)
    {
        var update = Builders<Message>.Update.Set(x => x.IsRead, true);
        var result = await _collection.UpdateOneAsync(x => x.Id == id, update);
        return result.ModifiedCount > 0;
    }
}
