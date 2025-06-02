using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Shift entities and their schedules.
    /// </summary>
    public interface IShiftRepository
    {
        /// <summary>
        /// Retrieves all shifts asynchronously.
        /// </summary>
        /// <returns>A list of all shifts.</returns>
        Task<IEnumerable<Shift>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific shift by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The shift ID.</param>
        /// <returns>The shift if found; otherwise null.</returns>
        Task<Shift?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new shift asynchronously.
        /// </summary>
        /// <param name="shift">The shift to add.</param>
        /// <returns>The added shift.</returns>
        Task<Shift> AddAsync(Shift shift);

        /// <summary>
        /// Updates an existing shift asynchronously.
        /// </summary>
        /// <param name="shift">The shift with updated data.</param>
        /// <returns>True if update succeeded; otherwise false.</returns>
        Task<bool> UpdateAsync(Shift shift);

        /// <summary>
        /// Deletes a shift by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The shift ID.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
