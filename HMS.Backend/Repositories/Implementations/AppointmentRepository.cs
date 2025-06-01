using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing Appointment entities.
    /// </summary>
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MyDbContext _context;

        public AppointmentRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Procedure)
                .Include(a => a.Room)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Procedure)
                .Include(a => a.Room)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <inheritdoc />
        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            var existingAppointment = await _context.Appointments.FindAsync(appointment.Id);
            if (existingAppointment == null) return false;

            _context.Entry(existingAppointment).CurrentValues.SetValues(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return false;

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
