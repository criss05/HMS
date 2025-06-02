using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Schedule entities in the data store.
    /// </summary>
    public interface IScheduleRepository
    {
        /// <summary>
        /// Gets all schedules asynchronously.
        /// </summary>
        /// <returns>A collection of all schedules.</returns>
        Task<IEnumerable<Schedule>> GetAllAsync();

        /// <summary>
        /// Gets a schedule by composite keys asynchronously.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <returns>The schedule if found, otherwise null.</returns>
        Task<Schedule?> GetByIdsAsync(int doctorId, int shiftId);

        /// <summary>
        /// Adds a new schedule asynchronously.
        /// </summary>
        /// <param name="schedule">The schedule to add.</param>
        /// <returns>The added schedule.</returns>
        Task<Schedule> AddAsync(Schedule schedule);

        /// <summary>
        /// Updates an existing schedule asynchronously.
        /// </summary>
        /// <param name="schedule">The schedule with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(Schedule schedule);

        /// <summary>
        /// Deletes a schedule asynchronously.
        /// </summary>
        /// <param name="doctorId">Doctor's id.</param>
        /// <param name="shiftId">Shift's id.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int doctorId, int shiftId);
    }
}
