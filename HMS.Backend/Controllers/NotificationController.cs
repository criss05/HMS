using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Backend.Controllers
{
    /// <summary>
    /// Controller for managing notifications via API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _repository;

        /// <summary>
        /// Constructor injecting the notification repository.
        /// </summary>
        /// <param name="repository">Notification repository instance.</param>
        public NotificationController(INotificationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all notifications.
        /// </summary>
        /// <returns>List of all notifications as NotificationDto.</returns>
        /// <response code="200">Returns list of notifications.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<NotificationDto>), 200)]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAll()
        {
            var notifications = await _repository.GetAllAsync();
            var dtos = new List<NotificationDto>();
            foreach (var notification in notifications)
            {
                dtos.Add(MapToDto(notification));
            }
            return Ok(dtos);
        }

        /// <summary>
        /// Retrieves a specific notification by id.
        /// </summary>
        /// <param name="id">Notification's unique id.</param>
        /// <returns>The requested NotificationDto.</returns>
        /// <response code="200">Returns the notification.</response>
        /// <response code="404">If notification not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(NotificationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationDto>> GetById(int id)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification == null)
                return NotFound();

            return Ok(MapToDto(notification));
        }

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        /// <param name="dto">Notification DTO to create.</param>
        /// <returns>The newly created NotificationDto.</returns>
        /// <response code="201">Returns the created notification.</response>
        /// <response code="400">If input data is invalid.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(NotificationDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<NotificationDto>> Create([FromBody] NotificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var notification = MapToEntity(dto);
            await _repository.AddAsync(notification);

            return CreatedAtAction(nameof(GetById), new { id = notification.Id }, MapToDto(notification));
        }

        /// <summary>
        /// Updates an existing notification.
        /// </summary>
        /// <param name="id">Id of the notification to update.</param>
        /// <param name="dto">Notification DTO with updated information.</param>
        /// <response code="204">Update successful.</response>
        /// <response code="400">If data invalid or id mismatch.</response>
        /// <response code="404">If notification not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] NotificationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Id in URL and payload do not match");

            var existingNotification = await _repository.GetByIdAsync(id);
            if (existingNotification == null)
                return NotFound();

            existingNotification.UserId = dto.UserId;
            existingNotification.Message = dto.Message;
            existingNotification.DeliveryDateTime = dto.DeliveryDateTime;

            await _repository.UpdateAsync(existingNotification);

            return NoContent();
        }

        /// <summary>
        /// Deletes a notification by id.
        /// </summary>
        /// <param name="id">Id of the notification to delete.</param>
        /// <response code="204">Deletion successful.</response>
        /// <response code="404">If notification not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingNotification = await _repository.GetByIdAsync(id);
            if (existingNotification == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Maps a Notification entity to NotificationDto.
        /// </summary>
        /// <param name="notification">Notification entity.</param>
        /// <returns>NotificationDto.</returns>
        private static NotificationDto MapToDto(Notification notification) =>
            new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Message = notification.Message,
                DeliveryDateTime = notification.DeliveryDateTime
            };

        /// <summary>
        /// Maps a NotificationDto to Notification entity.
        /// </summary>
        /// <param name="dto">Notification DTO.</param>
        /// <returns>Notification entity.</returns>
        private static Notification MapToEntity(NotificationDto dto) =>
            new Notification
            {
                Id = dto.Id,
                UserId = dto.UserId,
                Message = dto.Message,
                DeliveryDateTime = dto.DeliveryDateTime
            };
    }
}
