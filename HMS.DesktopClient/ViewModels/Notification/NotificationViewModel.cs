using HMS.Shared.DTOs;
using HMS.Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.DesktopClient.ViewModels.Notification
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        private NotificationService _notificationService;

        public NotificationViewModel(NotificationService notificationService)
        {
            this._notificationService = notificationService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId)
        {
            try
            {
                return await _notificationService.GetNotificationsByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve notifications: " + ex.Message);
            }
        }
    }
}
