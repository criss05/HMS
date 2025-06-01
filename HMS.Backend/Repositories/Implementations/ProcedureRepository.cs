using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Repository implementation for managing Procedure entities.
    /// </summary>
    public class ProcedureRepository : IProcedureRepository
    {
        private readonly MyDbContext _context;

        public ProcedureRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Procedure>> GetAllAsync()
        {
            return await _context.Procedures
                .Include(p => p.Department)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Procedure?> GetByIdAsync(int id)
        {
            return await _context.Procedures
                .Include(p => p.Department)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc />
        public async Task<Procedure> AddAsync(Procedure procedure)
        {
            _context.Procedures.Add(procedure);
            await _context.SaveChangesAsync();
            return procedure;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Procedure procedure)
        {
            var existingProcedure = await _context.Procedures.FindAsync(procedure.Id);
            if (existingProcedure == null) return false;

            _context.Entry(existingProcedure).CurrentValues.SetValues(procedure);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(int id)
        {
            var procedure = await _context.Procedures.FindAsync(id);
            if (procedure == null) return false;

            _context.Procedures.Remove(procedure);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
