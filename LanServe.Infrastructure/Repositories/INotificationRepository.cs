using LanServe.Domain.Entities;

namespace LanServe.Infrastructure.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserAsync(string userId, int take = 20);
    Task MarkAllReadAsync(string userId);
}
