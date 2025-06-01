using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this._context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await this._context.Users.FindAsync(id);
        }

        public async Task AddAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await this._context.Users.FindAsync(id);
            if (user != null)
            {
                this._context.Users.Remove(user);
                await this._context.SaveChangesAsync();
            }
        }
    }
}
