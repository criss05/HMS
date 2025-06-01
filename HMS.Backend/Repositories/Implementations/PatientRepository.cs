using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.Entities;
using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Repositories.Implementations
{
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
            return await _context.Patients.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient != null)
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
            }
        }
    }
}
