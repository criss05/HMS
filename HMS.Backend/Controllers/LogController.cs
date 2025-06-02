using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;

        public LogController(ILogRepository logRepository, IUserRepository userRepository)
        {
            _logRepository = logRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves all logs.
        /// </summary>
        /// <returns>List of logs.</returns>
        /// <response code="200">Returns the list of logs.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Log>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _logRepository.GetAllAsync();
            return Ok(logs);
        }

        /// <summary>
        /// Retrieves a specific log by ID.
        /// </summary>
        /// <param name="id">Log ID.</param>
        /// <returns>The requested log.</returns>
        /// <response code="200">Returns the requested log.</response>
        /// <response code="404">If the log is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Log), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var log = await _logRepository.GetByIdAsync(id);
            if (log == null) return NotFound();
            return Ok(log);
        }

        /// <summary>
        /// Creates a new log.
        /// </summary>
        /// <param name="dto">Log DTO to create.</param>
        /// <returns>The created log.</returns>
        /// <response code="201">Returns the newly created log.</response>
        /// <response code="400">If input data is invalid.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Log), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] LogDto dto)
        {
            // Validate user existence
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                return BadRequest($"User with ID {dto.UserId} not found.");

            var log = new Log
            {
                UserId = dto.UserId,
                User = user,
                Action = dto.Action,
                CreatedAt = dto.CreatedAt == default ? System.DateTime.UtcNow : dto.CreatedAt
            };

            var createdLog = await _logRepository.AddAsync(log);
            return CreatedAtAction(nameof(GetById), new { id = createdLog.Id }, createdLog);
        }

        /// <summary>
        /// Updates an existing log.
        /// </summary>
        /// <param name="id">Log ID.</param>
        /// <param name="dto">Log DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input data is invalid.</response>
        /// <response code="404">If log is not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] LogDto dto)
        {
            var existing = await _logRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                return BadRequest($"User with ID {dto.UserId} not found.");

            existing.UserId = dto.UserId;
            existing.User = user;
            existing.Action = dto.Action;
            existing.CreatedAt = dto.CreatedAt == default ? existing.CreatedAt : dto.CreatedAt;

            var success = await _logRepository.UpdateAsync(existing);
            if (!success)
                return BadRequest("Failed to update the log.");

            return NoContent();
        }

        /// <summary>
        /// Deletes a log.
        /// </summary>
        /// <param name="id">Log ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If log is not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _logRepository.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
