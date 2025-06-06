using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing Patient entities.
    /// </summary>
    public class PatientRepository : IPatientRepository
    {
        private readonly MyDbContext _context;

        public PatientRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients
                .Include(p => p.Reviews)
                .Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients
                .Include(p => p.Reviews)
                .Include(p => p.Appointments)
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc />
        public async Task<Patient> AddAsync(Patient patient)
        {
            bool exists = await _context.Patients.AnyAsync(p =>
        p.Email == patient.Email || p.CNP == patient.CNP);

            if (exists)
                throw new Exception("A patient with the same email or CNP already exists.");

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Patient patient)
        {
            var existingPatient = await _context.Patients.FindAsync(patient.Id);
            if (existingPatient == null) return false;

            _context.Entry(existingPatient).CurrentValues.SetValues(patient);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
