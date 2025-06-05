using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.Entities;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface defining data operations for Admin entities.
    /// </summary>
    public interface IAdminRepository
    {
        /// <summary>
        /// Retrieves all admins asynchronously.
        /// </summary>
        /// <returns>A collection of all admins.</returns>
        Task<IEnumerable<Admin>> GetAllAsync();

        /// <summary>
        /// Retrieves an admin by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The admin's unique ID.</param>
        /// <returns>The matching admin or null if not found.</returns>
        Task<Admin?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves an admin by email asynchronously.
        /// </summary>
        /// <param name="email">The admin's email.</param>
        /// <returns>The matching admin or null if not found.</returns>
        Task<Admin?> GetByEmailAsync(string email);

        /// <summary>
        /// Adds a new admin asynchronously.
        /// </summary>
        /// <param name="admin">Admin entity to add.</param>
        Task AddAsync(Admin admin);

        /// <summary>
        /// Updates an existing admin asynchronously.
        /// </summary>
        /// <param name="admin">Admin entity with updated data.</param>
        Task UpdateAsync(Admin admin);

        /// <summary>
        /// Deletes an admin by id asynchronously.
        /// </summary>
        /// <param name="id">The unique ID of the admin to delete.</param>
        Task DeleteAsync(int id);
    }
}
