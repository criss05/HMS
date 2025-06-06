using HMS.Shared.DTOs;
using HMS.Shared.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Services
{
    public class NotificationService
    {
        private INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            this._notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            try
            {
                return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve notifications: " + ex.Message);
            }
        }
    }
}
