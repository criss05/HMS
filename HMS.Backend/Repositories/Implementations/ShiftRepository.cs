using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing Shift entities.
    /// </summary>
    public class ShiftRepository : IShiftRepository
    {
        private readonly MyDbContext _context;

        public ShiftRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Shift>> GetAllAsync()
        {
            return await _context.Shifts
                .Include(s => s.Schedules)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Shift?> GetByIdAsync(int id)
        {
            return await _context.Shifts
                .Include(s => s.Schedules)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <inheritdoc />
        public async Task<Shift> AddAsync(Shift shift)
        {
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();
            return shift;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Shift shift)
        {
            var existingShift = await _context.Shifts
                .Include(s => s.Schedules)
                .FirstOrDefaultAsync(s => s.Id == shift.Id);

            if (existingShift == null) return false;

            // Update scalar properties
            existingShift.Date = shift.Date;
            existingShift.StartTime = shift.StartTime;
            existingShift.EndTime = shift.EndTime;

            // Optionally update schedules here if needed (handled in controller usually)

            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var shift = await _context.Shifts.FindAsync(id);
            if (shift == null) return false;

            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
