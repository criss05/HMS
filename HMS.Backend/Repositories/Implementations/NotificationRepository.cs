using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Implementation of INotificationRepository to manage Notification data operations.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly MyDbContext _context;

        /// <summary>
        /// Constructor injecting the database context.
        /// </summary>
        /// <param name="context">Database context instance.</param>
        public NotificationRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await _context.Notifications.Include(n => n.User).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.Include(n => n.User).FirstOrDefaultAsync(n => n.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}
