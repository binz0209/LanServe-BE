using LanServe.Domain.Entities;
using MongoDB.Driver;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IMongoCollection<Notification> collection) : base(collection) { }

        // Lấy một số notification (limit)
        public async Task<IEnumerable<Notification>> GetByUserAsync(string userId, int take = 20)
        {
            var list = await _collection.Find(n => n.UserId == userId)
                                        .SortByDescending(n => n.CreatedAt)
                                        .Limit(take)
                                        .ToListAsync();
            return list.AsEnumerable();
        }

        // Đúng tên method interface yêu cầu
        public async Task MarkAsReadAsync(string userId)
        {
            await _collection.UpdateManyAsync(
                n => n.UserId == userId && !n.IsRead,
                Builders<Notification>.Update.Set(x => x.IsRead, true)
            );
        }


        // Bổ sung method còn thiếu
        public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(string userId)
        {
            var list = await _collection.Find(n => n.UserId == userId)
                                        .SortByDescending(n => n.CreatedAt)
                                        .ToListAsync();
            return list;
        }
    }
}
