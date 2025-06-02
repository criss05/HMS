using System;

namespace HMS.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for Log entity.
    /// </summary>
    public class LogDto
    {
        /// <summary>
        /// Gets or sets the log ID.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the User ID associated with the log.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the action description.
        /// </summary>
        public string Action { get; set; } = null!;

        /// <summary>
        /// Gets or sets the creation timestamp of the log.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
