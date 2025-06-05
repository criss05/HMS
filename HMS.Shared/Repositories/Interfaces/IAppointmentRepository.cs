using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Shared.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Appointment entities in the data store.
    /// </summary>
    public interface IAppointmentRepository
    {
        /// <summary>
        /// Gets all appointments asynchronously.
        /// </summary>
        /// <returns>A collection of all appointments.</returns>
        Task<IEnumerable<Appointment>> GetAllAsync();

        /// <summary>
        /// Gets an appointment by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the appointment.</param>
        /// <returns>The appointment if found; otherwise null.</returns>
        Task<Appointment?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new appointment asynchronously.
        /// </summary>
        /// <param name="appointment">The appointment to add.</param>
        /// <returns>The added appointment.</returns>
        Task<Appointment> AddAsync(Appointment appointment);

        /// <summary>
        /// Updates an existing appointment asynchronously.
        /// </summary>
        /// <param name="appointment">The appointment with updated data.</param>
        /// <returns>True if update succeeded; otherwise false.</returns>
        Task<bool> UpdateAsync(Appointment appointment);

        /// <summary>
        /// Deletes an appointment by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the appointment to delete.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
