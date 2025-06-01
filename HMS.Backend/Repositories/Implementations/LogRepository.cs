using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Provides access to log-related data operations.
    /// </summary>
    public class LogRepository : ILogRepository
    {
        private readonly MyDbContext _context;

        public LogRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            return await _context.Logs.Include(l => l.User).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Log?> GetByIdAsync(int id)
        {
            return await _context.Logs.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);
        }

        /// <inheritdoc />
        public async Task<Log> AddAsync(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            return log;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Log log)
        {
            var existingLog = await _context.Logs.FindAsync(log.Id);
            if (existingLog == null)
                return false;

            existingLog.Action = log.Action;
            existingLog.UserId = log.UserId;
            existingLog.CreatedAt = log.CreatedAt;

            _context.Logs.Update(existingLog);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
                return false;

            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
