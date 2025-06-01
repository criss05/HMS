using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.Entities;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for managing User entities.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A collection of all users.</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a user by their email address asynchronously.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(User user);

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="user">The user with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(User user);

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(int id);
    }
}
