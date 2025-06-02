using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Repositories
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly MyDbContext _context;

        public EquipmentRepository(MyDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Equipment>> GetAllAsync()
        {
            return await _context.Equipments.Include(e => e.Rooms).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Equipment?> GetByIdAsync(int id)
        {
            return await _context.Equipments.Include(e => e.Rooms)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Equipment equipment)
        {
            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Equipment equipment)
        {
            _context.Equipments.Update(equipment);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment != null)
            {
                _context.Equipments.Remove(equipment);
                await _context.SaveChangesAsync();
            }
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Equipments.AnyAsync(e => e.Id == id);
        }
    }
}
