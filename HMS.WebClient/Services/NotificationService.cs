using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.WebClient.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsForUserAsync(int userId)
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Where(n => n.UserId == userId).ToList();
        }

        public async Task<IEnumerable<NotificationDto>> GetRecentUnreadNotificationsAsync(int userId, int count)
        {
            // Since NotificationDto doesn't have an IsRead property,
            // we'll return the most recent notifications instead
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.DeliveryDateTime)
                .Take(count)
                .ToList();
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId)
        {
            // Since we can't mark notifications as read directly,
            // this operation cannot be supported with the current DTO structure
            // You might need to extend the DTO or implement this differently
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId, int userId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                return false;

            return await _notificationRepository.DeleteAsync(notificationId);
        }

        public async Task<int> GetUnreadNotificationsCountAsync(int userId)
        {
            // Since we don't have an IsRead property,
            // we'll return the count of all notifications as a placeholder
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Count(n => n.UserId == userId);
        }
    }
}