using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Log repository handling CRUD operations.
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Gets all logs.
        /// </summary>
        /// <returns>A collection of all Log entities.</returns>
        Task<IEnumerable<Log>> GetAllAsync();

        /// <summary>
        /// Gets a log by ID.
        /// </summary>
        /// <param name="id">The ID of the log.</param>
        /// <returns>The log with the specified ID or null if not found.</returns>
        Task<Log?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new log.
        /// </summary>
        /// <param name="log">The log entity to add.</param>
        /// <returns>The added log entity.</returns>
        Task<Log> AddAsync(Log log);

        /// <summary>
        /// Updates an existing log.
        /// </summary>
        /// <param name="log">The log entity with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        Task<bool> UpdateAsync(Log log);

        /// <summary>
        /// Deletes a log by ID.
        /// </summary>
        /// <param name="id">The ID of the log to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
