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
    public class ProcedureController : ControllerBase
    {
        private readonly IProcedureRepository _procedureRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public ProcedureController(IProcedureRepository procedureRepository, IDepartmentRepository departmentRepository)
        {
            _procedureRepository = procedureRepository;
            _departmentRepository = departmentRepository;
        }

        /// <summary>
        /// Retrieves all procedures.
        /// </summary>
        /// <returns>List of procedures.</returns>
        /// <response code="200">Returns the list of procedures.</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Procedure>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var procedures = await _procedureRepository.GetAllAsync();
            return Ok(procedures);
        }

        /// <summary>
        /// Retrieves a specific procedure by ID.
        /// </summary>
        /// <param name="id">Procedure's ID.</param>
        /// <returns>The requested procedure.</returns>
        /// <response code="200">Returns the requested procedure.</response>
        /// <response code="404">If the procedure is not found.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Procedure), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var procedure = await _procedureRepository.GetByIdAsync(id);
            if (procedure == null) return NotFound();
            return Ok(procedure);
        }

        /// <summary>
        /// Creates a new procedure.
        /// </summary>
        /// <param name="dto">Procedure DTO to create.</param>
        /// <returns>The created procedure.</returns>
        /// <response code="201">Returns the newly created procedure.</response>
        /// <response code="400">If the procedure object is invalid.</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Procedure), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProcedureDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                return BadRequest($"Department with ID {dto.DepartmentId} not found.");

            var procedure = new Procedure
            {
                DepartmentId = dto.DepartmentId,
                Department = department,
                Name = dto.Name,
                Duration = dto.Duration
            };

            var createdProcedure = await _procedureRepository.AddAsync(procedure);
            return CreatedAtAction(nameof(GetById), new { id = createdProcedure.Id }, createdProcedure);
        }

        /// <summary>
        /// Updates an existing procedure.
        /// </summary>
        /// <param name="id">Procedure's ID.</param>
        /// <param name="dto">Procedure DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input is invalid.</response>
        /// <response code="404">If procedure not found.</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ProcedureDto dto)
        {
            var existing = await _procedureRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                return BadRequest($"Department with ID {dto.DepartmentId} not found.");

            existing.DepartmentId = dto.DepartmentId;
            existing.Department = department;
            existing.Name = dto.Name;
            existing.Duration = dto.Duration;

            await _procedureRepository.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Deletes a procedure.
        /// </summary>
        /// <param name="id">Procedure's ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If procedure not found.</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _procedureRepository.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
