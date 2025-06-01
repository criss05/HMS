using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for managing Review entities in the data store.
    /// </summary>
    public interface IReviewRepository
    {
        /// <summary>
        /// Gets all reviews asynchronously.
        /// </summary>
        /// <returns>A collection of all reviews.</returns>
        Task<IEnumerable<Review>> GetAllAsync();

        /// <summary>
        /// Gets a review by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>The review if found; otherwise null.</returns>
        Task<Review?> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new review asynchronously.
        /// </summary>
        /// <param name="review">The review to add.</param>
        /// <returns>The added review.</returns>
        Task<Review> AddAsync(Review review);

        /// <summary>
        /// Updates an existing review asynchronously.
        /// </summary>
        /// <param name="review">The review with updated data.</param>
        /// <returns>True if update succeeded; otherwise false.</returns>
        Task<bool> UpdateAsync(Review review);

        /// <summary>
        /// Deletes a review by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        /// <returns>True if deletion succeeded; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
