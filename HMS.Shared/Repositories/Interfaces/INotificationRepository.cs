using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface defining data operations for Notification entities.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Retrieves all notifications asynchronously.
        /// </summary>
        /// <returns>A collection of all notifications.</returns>
        Task<IEnumerable<NotificationDto>> GetAllAsync();

        /// <summary>
        /// Retrieves a notification by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The notification's unique ID.</param>
        /// <returns>The matching notification or null if not found.</returns>
        Task<Notification?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="notification">Notification entity to add.</param>
        Task<Notification> AddAsync(Notification notification);

        /// <summary>
        /// Updates an existing notification asynchronously.
        /// </summary>
        /// <param name="notification">Notification entity with updated data.</param>
        Task<bool> UpdateAsync(Notification notification);

        /// <summary>
        /// Deletes a notification by id asynchronously.
        /// </summary>
        /// <param name="id">The unique ID of the notification to delete.</param>
        Task<bool> DeleteAsync(int id);
    }
}
