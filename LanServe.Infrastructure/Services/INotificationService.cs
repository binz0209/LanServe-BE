using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanServe.Infrastructure.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateAsync(Notification notification);
        Task<IEnumerable<Notification>> GetByUserIdAsync(string userId);
        Task MarkAsReadAsync(string id);
    }
}
