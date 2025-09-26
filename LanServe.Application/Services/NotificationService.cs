using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanServe.Application.Interfaces.Services;
using LanServe.Domain.Entities;
using LanServe.Application.Interfaces.Repositories;

namespace LanServe.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            await _notificationRepository.AddAsync(notification);
            return notification;
        }

        public async Task<IEnumerable<Notification>> GetByUserIdAsync(string userId)
            => await _notificationRepository.GetByUserIdAsync(userId);

        public async Task MarkAsReadAsync(string id)
            => await _notificationRepository.MarkAsReadAsync(id);
    }
}
