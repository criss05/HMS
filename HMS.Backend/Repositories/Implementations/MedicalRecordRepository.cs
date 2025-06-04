using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing MedicalRecord entities.
    /// </summary>
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly MyDbContext _context;

        public MedicalRecordRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MedicalRecord>> GetAllAsync()
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Reviews)
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Appointments)
                .Include(m => m.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Department)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Schedules)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Reviews)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Appointments)
                .Include(m => m.Procedure)
                    .ThenInclude(p => p.Department)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<MedicalRecord?> GetByIdAsync(int id)
        {
            return await _context.MedicalRecords
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Reviews)
                .Include(m => m.Patient)
                    .ThenInclude(p => p.Appointments)
                .Include(m => m.Patient)
                    .ThenInclude(p => p.MedicalRecords)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Department)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Schedules)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Reviews)
                .Include(m => m.Doctor)
                    .ThenInclude(d => d.Appointments)
                .Include(m => m.Procedure)
                    .ThenInclude(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        /// <inheritdoc />
        public async Task<MedicalRecord> AddAsync(MedicalRecord medicalRecord)
        {
            _context.MedicalRecords.Add(medicalRecord);
            await _context.SaveChangesAsync();
            return medicalRecord;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(MedicalRecord medicalRecord)
        {
            var existingRecord = await _context.MedicalRecords.FindAsync(medicalRecord.Id);
            if (existingRecord == null) return false;

            _context.Entry(existingRecord).CurrentValues.SetValues(medicalRecord);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var record = await _context.MedicalRecords.FindAsync(id);
            if (record == null) return false;

            _context.MedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
