using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for Equipment entity operations.
    /// </summary>
    public interface IEquipmentRepository
    {
        /// <summary>
        /// Gets all equipments.
        /// </summary>
        /// <returns>List of Equipment entities.</returns>
        Task<List<Equipment>> GetAllAsync();

        /// <summary>
        /// Gets equipment by id.
        /// </summary>
        /// <param name="id">Equipment id.</param>
        /// <returns>Equipment entity or null.</returns>
        Task<Equipment?> GetByIdAsync(int id);

        /// <summary>
        /// Adds new equipment.
        /// </summary>
        /// <param name="equipment">Equipment entity to add.</param>
        Task AddAsync(Equipment equipment);

        /// <summary>
        /// Updates existing equipment.
        /// </summary>
        /// <param name="equipment">Equipment entity to update.</param>
        Task UpdateAsync(Equipment equipment);

        /// <summary>
        /// Deletes equipment by id.
        /// </summary>
        /// <param name="id">Equipment id.</param>
        Task DeleteAsync(int id);

        /// <summary>
        /// Checks if equipment exists.
        /// </summary>
        /// <param name="id">Equipment id.</param>
        /// <returns>True if exists.</returns>
        Task<bool> ExistsAsync(int id);
    }

}
