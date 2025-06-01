using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>List of all departments.</returns>
        /// <response code="200">Returns the list of departments.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Department>), 200)]
        public async Task<ActionResult<IEnumerable<Department>>> GetAll()
        {
            var departments = await _repository.GetAllAsync();
            return Ok(departments);
        }

        /// <summary>
        /// Retrieves a specific department by unique id.
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <returns>The requested department.</returns>
        /// <response code="200">Returns the department.</response>
        /// <response code="404">If department with specified id is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Department), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Department>> GetById(int id)
        {
            var department = await _repository.GetByIdAsync(id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }

        /// <summary>
        /// Creates a new department.
        /// </summary>
        /// <param name="department">Department object to create.</param>
        /// <returns>Newly created department.</returns>
        /// <response code="201">Returns the newly created department.</response>
        /// <response code="400">If the department data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Department), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Department>> Create([FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDepartment = await _repository.AddAsync(department);
            return CreatedAtAction(nameof(GetById), new { id = createdDepartment.Id }, createdDepartment);
        }

        /// <summary>
        /// Updates an existing department.
        /// </summary>
        /// <param name="id">ID of the department to update.</param>
        /// <param name="department">Updated department object.</param>
        /// <response code="204">If update is successful (No Content).</response>
        /// <response code="400">If the input data is invalid or id mismatch.</response>
        /// <response code="404">If department with specified id is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != department.Id)
                return BadRequest("Id in URL and payload do not match");

            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
                return NotFound();

            var updated = await _repository.UpdateAsync(department);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a department by id.
        /// </summary>
        /// <param name="id">ID of the department to delete.</param>
        /// <response code="204">If deletion is successful (No Content).</response>
        /// <response code="404">If department with specified id is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _repository.GetByIdAsync(id);
            if (exists == null)
                return NotFound();

            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
