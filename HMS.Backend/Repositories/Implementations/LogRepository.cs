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

        public async Task<IEnumerable<Log>> GetAllAsync()
        {
            return await _context.Logs.Include(l => l.User).ToListAsync();
        }

        public async Task<Log?> GetByIdAsync(int id)
        {
            return await _context.Logs.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddAsync(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log != null)
            {
                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}
