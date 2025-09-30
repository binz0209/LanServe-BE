using LanServe.Application.Interfaces.Repositories;
using LanServe.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly IMongoCollection<Message> _col;

    public MessageRepository(IMongoCollection<Message> collection)
    {
        _col = collection;
    }

    public async Task<Message> AddAsync(Message message)
    {
        await _col.InsertOneAsync(message);
        return message;
    }

    public async Task<bool> MarkAsReadAsync(string id)
    {
        var res = await _col.UpdateOneAsync(
            Builders<Message>.Filter.Eq(x => x.Id, id),
            Builders<Message>.Update.Set(x => x.IsRead, true)
        );
        return res.ModifiedCount > 0;
    }

    public async Task<List<Message>> GetByConversationAsync(string conversationKey)
    {
        var cur = await _col.Find(x => x.ConversationKey == conversationKey)
                            .SortBy(x => x.CreatedAt)
                            .ToListAsync();
        return cur;
    }

    public async Task<List<Message>> GetByProjectAsync(string projectId)
    {
        var cur = await _col.Find(x => x.ProjectId == projectId)
                            .SortBy(x => x.CreatedAt)
                            .ToListAsync();
        return cur;
    }

    public async Task<List<Message>> GetByUserAsync(string userId)
    {
        var filter = Builders<Message>.Filter.Or(
            Builders<Message>.Filter.Eq(x => x.SenderId, userId),
            Builders<Message>.Filter.Eq(x => x.ReceiverId, userId)
        );
        var cur = await _col.Find(filter)
                            .SortBy(x => x.CreatedAt)
                            .ToListAsync();
        return cur;
    }

    // Mongo aggregation để trả danh sách hội thoại gọn nhẹ
    public async Task<List<(string ConversationKey, string PartnerId, string LastMessage, DateTime LastAt, int UnreadCount)>>
        GetConversationsForUserAsync(string userId)
    {
        var match = Builders<Message>.Filter.Or(
            Builders<Message>.Filter.Eq(x => x.SenderId, userId),
            Builders<Message>.Filter.Eq(x => x.ReceiverId, userId)
        );

        var pipeline = _col.Aggregate()
            .Match(match)
            .SortBy(m => m.CreatedAt) // cần để $last hoạt động chuẩn
            .Group(new BsonDocument
            {
                { "_id", "$ConversationKey" },
                { "lastMessage", new BsonDocument("$last", "$Text") },
                { "lastAt", new BsonDocument("$last", "$CreatedAt") },
                { "lastSender", new BsonDocument("$last", "$SenderId") },
                { "lastReceiver", new BsonDocument("$last", "$ReceiverId") },
                { "unreadCount", new BsonDocument("$sum", new BsonDocument("$cond", new BsonArray{
                    new BsonDocument("$and", new BsonArray{
                        new BsonDocument("$eq", new BsonArray{"$ReceiverId", userId}),
                        new BsonDocument("$eq", new BsonArray{"$IsRead", false})
                    }),
                    1, 0
                })) }
            })
            .Project(new BsonDocument
            {
                { "conversationKey", "$_id" },
                { "lastMessage", "$lastMessage" },
                { "lastAt", "$lastAt" },
                { "partnerId", new BsonDocument("$cond", new BsonArray{
                    new BsonDocument("$eq", new BsonArray{"$lastSender", userId}),
                    "$lastReceiver",
                    "$lastSender"
                }) },
                { "unreadCount", "$unreadCount" },
                { "_id", 0 }
            })
            .Sort(new BsonDocument("lastAt", -1));

        var docs = await pipeline.ToListAsync();

        // map sang tuple (không phải DTO)
        var list = docs.Select(d => (
            ConversationKey: d["conversationKey"].AsString,
            PartnerId: d.GetValue("partnerId", BsonNull.Value).IsBsonNull ? "" : d["partnerId"].AsString,
            LastMessage: d.GetValue("lastMessage", BsonNull.Value).IsBsonNull ? "" : d["lastMessage"].AsString,
            LastAt: d.GetValue("lastAt", BsonNull.Value).IsBsonNull ? DateTime.MinValue : d["lastAt"].ToUniversalTime(),
            UnreadCount: d.GetValue("unreadCount", 0).ToInt32()
        )).ToList();

        return list;
    }
}
