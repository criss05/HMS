using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for performing CRUD operations on logs.
    /// </summary>
    public interface ILogRepository
    {
        /// <summary>
        /// Retrieves all logs from the database.
        /// </summary>
        /// <returns>A list of log entries.</returns>
        Task<IEnumerable<Log>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific log by its ID.
        /// </summary>
        /// <param name="id">The ID of the log to retrieve.</param>
        /// <returns>The log entry if found; otherwise, null.</returns>
        Task<Log?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new log entry to the database.
        /// </summary>
        /// <param name="log">The log entry to add.</param>
        Task AddAsync(Log log);

        /// <summary>
        /// Deletes a log entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the log to delete.</param>
        Task DeleteAsync(int id);
    }
}
