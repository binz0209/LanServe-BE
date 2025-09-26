using LanServe.Domain.Entities;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(IMongoCollection<Notification> collection) : base(collection) { }

    public Task<IEnumerable<Notification>> GetByUserAsync(string userId, int take = 20)
        => _collection.Find(n => n.UserId == userId)
                      .SortByDescending(n => n.CreatedAt)
                      .Limit(take)
                      .ToListAsync()
                      .ContinueWith(task => task.Result.AsEnumerable());

    public Task MarkAllReadAsync(string userId)
        => _collection.UpdateManyAsync(n => n.UserId == userId && !n.IsRead,
                                       Builders<Notification>.Update.Set(x => x.IsRead, true));
}
