using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.Entities;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Patient entity data access operations.
    /// Defines asynchronous CRUD methods.
    /// </summary>
    public interface IPatientRepository
    {
        /// <summary>
        /// Retrieves all patients from the database.
        /// </summary>
        /// <returns>A list of patients.</returns>
        Task<IEnumerable<Patient>> GetAllAsync();

        /// <summary>
        /// Retrieves a patient by their unique ID.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <returns>The patient if found, otherwise null.</returns>
        Task<Patient?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new patient to the database.
        /// </summary>
        /// <param name="patient">The patient to add.</param>
        Task AddAsync(Patient patient);

        /// <summary>
        /// Updates an existing patient's data.
        /// </summary>
        /// <param name="patient">The patient with updated values.</param>
        Task UpdateAsync(Patient patient);

        /// <summary>
        /// Deletes a patient by their ID.
        /// </summary>
        /// <param name="id">The ID of the patient to delete.</param>
        Task DeleteAsync(int id);
    }
}
