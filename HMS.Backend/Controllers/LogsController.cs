using Microsoft.AspNetCore.Mvc;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepository;

        public LogsController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        /// <summary>
        /// Retrieves all log entries.
        /// </summary>
        /// <returns>A list of logs.</returns>
        /// <response code="200">Returns the list of logs</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetAll()
        {
            var logs = await _logRepository.GetAllAsync();
            return Ok(logs);
        }

        /// <summary>
        /// Retrieves a specific log entry by ID.
        /// </summary>
        /// <param name="id">The ID of the log entry.</param>
        /// <returns>The requested log entry.</returns>
        /// <response code="200">Returns the log entry</response>
        /// <response code="404">If the log is not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> GetById(int id)
        {
            var log = await _logRepository.GetByIdAsync(id);
            if (log == null)
                return NotFound();

            return Ok(log);
        }

        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="log">The log to create.</param>
        /// <returns>The created log entry with its location.</returns>
        /// <response code="201">Returns the newly created log</response>
        /// <response code="400">If the log is invalid</response>
        [HttpPost]
        public async Task<IActionResult> Create(Log log)
        {
            await _logRepository.AddAsync(log);
            return CreatedAtAction(nameof(GetById), new { id = log.Id }, log);
        }

        /// <summary>
        /// Deletes a log entry by ID.
        /// </summary>
        /// <param name="id">The ID of the log to delete.</param>
        /// <response code="204">Log successfully deleted</response>
        /// <response code="404">If the log is not found</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _logRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _logRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
