using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing Schedule entities.
    /// </summary>
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly MyDbContext _context;

        public ScheduleRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _context.Schedules
                .Include(s => s.Doctor)
                .Include(s => s.Shift)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Schedule?> GetByIdsAsync(int doctorId, int shiftId)
        {
            return await _context.Schedules
                .Include(s => s.Doctor)
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.ShiftId == shiftId);
        }

        /// <inheritdoc />
        public async Task<Schedule> AddAsync(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Schedule schedule)
        {
            var existing = await _context.Schedules
                .FirstOrDefaultAsync(s => s.DoctorId == schedule.DoctorId && s.ShiftId == schedule.ShiftId);
            if (existing == null) return false;

            _context.Entry(existing).CurrentValues.SetValues(schedule);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int doctorId, int shiftId)
        {
            var existing = await _context.Schedules
                .FirstOrDefaultAsync(s => s.DoctorId == doctorId && s.ShiftId == shiftId);
            if (existing == null) return false;

            _context.Schedules.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
