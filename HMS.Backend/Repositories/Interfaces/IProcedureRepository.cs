using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for Procedure repository handling CRUD operations.
    /// </summary>
    public interface IProcedureRepository
    {
        /// <summary>
        /// Gets all procedures.
        /// </summary>
        Task<IEnumerable<Procedure>> GetAllAsync();

        /// <summary>
        /// Gets a procedure by ID.
        /// </summary>
        /// <param name="id">The ID of the procedure to retrieve.</param>
        Task<Procedure?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new procedure.
        /// </summary>
        /// <param name="procedure">The procedure entity to add.</param>
        Task<Procedure> AddAsync(Procedure procedure);

        /// <summary>
        /// Updates an existing procedure.
        /// </summary>
        /// <param name="procedure">The procedure entity with updated information.</param>
        Task<bool> UpdateAsync(Procedure procedure);

        /// <summary>
        /// Deletes a procedure by ID.
        /// </summary>
        /// <param name="id">The ID of the procedure to delete.</param>
        Task<bool> DeleteAsync(int id);
    }
}
