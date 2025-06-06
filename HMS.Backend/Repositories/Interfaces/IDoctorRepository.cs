using HMS.Shared.DTOs.Doctor;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Doctor entities in the data store.
    /// </summary>
    public interface IDoctorRepository
    {
        /// <summary>
        /// Gets all doctors asynchronously.
        /// </summary>
        /// <returns>A collection of all doctors.</returns>
        Task<IEnumerable<Doctor>> GetAllAsync();

        /// <summary>
        /// Gets a doctor by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the doctor.</param>
        /// <returns>The doctor if found, otherwise null.</returns>
        Task<Doctor?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all doctors associated with a specific department asynchronously.
        /// </summary>
        /// <param name="departmentId">The unique  identifier of the department.</param>
        /// <returns>The doctors in the departnemt if found, otherwise empty list.</returns>
        Task<IEnumerable<Doctor>> GetByDepartmentIdAsync(int departmentId);

        /// <summary>
        /// Gets a list of doctor items for display purposes asynchronously.
        /// </summary>
        /// <returns>A collection of doctor list items.</returns>
        Task<IEnumerable<DoctorListItemDto>> GetDoctorListItemsAsync();

        /// <summary>
        /// Adds a new doctor asynchronously.
        /// </summary>
        /// <param name="doctor">The doctor to add.</param>
        /// <returns>The added doctor.</returns>
        Task<Doctor> AddAsync(Doctor doctor);

        /// <summary>
        /// Updates an existing doctor asynchronously.
        /// </summary>
        /// <param name="doctor">The doctor with updated data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateAsync(Doctor doctor);

        /// <summary>
        /// Deletes a doctor by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the doctor to delete.</param>
        /// <returns>True if deletion succeeded, false otherwise.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
