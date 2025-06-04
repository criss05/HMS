using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing MedicalRecord entities.
    /// </summary>
    public interface IMedicalRecordRepository
    {
        /// <summary>
        /// Gets all medical records asynchronously.
        /// </summary>
        /// <returns>A collection of all medical records.</returns>
        Task<IEnumerable<MedicalRecord>> GetAllAsync();

        /// <summary>
        /// Gets a medical record by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the medical record.</param>
        /// <returns>The medical record if found, otherwise null.</returns>
        Task<MedicalRecord?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new medical record asynchronously.
        /// </summary>
        /// <param name="medicalRecord">The medical record to add.</param>
        /// <returns>The added medical record.</returns>
        Task<MedicalRecord> AddAsync(MedicalRecord medicalRecord);

        /// <summary>
        /// Updates an existing medical record asynchronously.
        /// </summary>
        /// <param name="medicalRecord">The medical record with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(MedicalRecord medicalRecord);

        /// <summary>
        /// Deletes a medical record by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the medical record to delete.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
