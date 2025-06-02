using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Data;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMS.Backend.Repositories.Implementations
{
    /// <summary>
    /// Implementation of the IUserRepository interface for managing User entities.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        /// <summary>
        /// Initializes a new instance of the UserRepository class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UserRepository(MyDbContext context)
        {
            this._context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this._context.Users.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User?> GetByIdAsync(int id)
        {
            return await this._context.Users.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await this._context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <inheritdoc />
        public async Task AddAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(User user)
        {
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        /// <inheritdoc />
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
