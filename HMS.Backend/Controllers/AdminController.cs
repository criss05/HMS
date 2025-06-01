using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Backend.Controllers
{
    /// <summary>
    /// Controller for managing Admin entities via API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repository;

        /// <summary>
        /// Constructor with dependency injection of repository.
        /// </summary>
        /// <param name="repository">Repository for admin data operations.</param>
        public AdminController(IAdminRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all admins.
        /// </summary>
        /// <returns>List of all admins as AdminDto.</returns>
        /// <response code="200">Returns list of admins.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AdminDto>), 200)]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAll()
        {
            var admins = await _repository.GetAllAsync();
            var dtos = new List<AdminDto>();
            foreach (var admin in admins)
            {
                dtos.Add(MapToDto(admin));
            }
            return Ok(dtos);
        }

        /// <summary>
        /// Retrieves a specific admin by id.
        /// </summary>
        /// <param name="id">Admin's unique id.</param>
        /// <returns>The requested AdminDto.</returns>
        /// <response code="200">Returns the admin.</response>
        /// <response code="404">If admin not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AdminDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AdminDto>> GetById(int id)
        {
            var admin = await _repository.GetByIdAsync(id);
            if (admin == null)
                return NotFound();

            return Ok(MapToDto(admin));
        }

        /// <summary>
        /// Creates a new admin.
        /// </summary>
        /// <param name="dto">Admin DTO to create.</param>
        /// <returns>The newly created AdminDto.</returns>
        /// <response code="201">Returns the created admin.</response>
        /// <response code="400">If input data is invalid or email already in use.</response>
        [HttpPost]
        [ProducesResponseType(typeof(AdminDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AdminDto>> Create([FromBody] AdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingAdmin = await _repository.GetByEmailAsync(dto.Email);
            if (existingAdmin != null)
                return BadRequest($"Email '{dto.Email}' is already in use.");

            var admin = MapToEntity(dto);
            await _repository.AddAsync(admin);

            return CreatedAtAction(nameof(GetById), new { id = admin.Id }, MapToDto(admin));
        }

        /// <summary>
        /// Updates an existing admin.
        /// </summary>
        /// <param name="id">Id of the admin to update.</param>
        /// <param name="dto">Admin DTO with updated information.</param>
        /// <response code="204">Update successful.</response>
        /// <response code="400">If data invalid, id mismatch, or email in use.</response>
        /// <response code="404">If admin not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] AdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Id in URL and payload do not match");

            var existingAdmin = await _repository.GetByIdAsync(id);
            if (existingAdmin == null)
                return NotFound();

            var adminWithEmail = await _repository.GetByEmailAsync(dto.Email);
            if (adminWithEmail != null && adminWithEmail.Id != id)
                return BadRequest($"Email '{dto.Email}' is already in use by another admin.");

            // Update properties
            existingAdmin.Email = dto.Email;
            existingAdmin.Password = dto.Password; // Handle hashing as needed
            existingAdmin.Role = dto.Role;
            existingAdmin.Name = dto.Name;
            existingAdmin.CNP = dto.CNP;
            existingAdmin.PhoneNumber = dto.PhoneNumber;
            existingAdmin.CreatedAt = dto.CreatedAt;

            await _repository.UpdateAsync(existingAdmin);

            return NoContent();
        }

        /// <summary>
        /// Deletes an admin by id.
        /// </summary>
        /// <param name="id">Id of the admin to delete.</param>
        /// <response code="204">Deletion successful.</response>
        /// <response code="404">If admin not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingAdmin = await _repository.GetByIdAsync(id);
            if (existingAdmin == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Maps an Admin entity to AdminDto.
        /// </summary>
        /// <param name="admin">Admin entity.</param>
        /// <returns>AdminDto.</returns>
        private AdminDto MapToDto(Admin admin) =>
            new AdminDto
            {
                Id = admin.Id,
                Email = admin.Email,
                Password = admin.Password,
                Role = admin.Role,
                Name = admin.Name,
                CNP = admin.CNP,
                PhoneNumber = admin.PhoneNumber,
                CreatedAt = admin.CreatedAt
            };

        /// <summary>
        /// Maps an AdminDto to Admin entity.
        /// </summary>
        /// <param name="dto">Admin DTO.</param>
        /// <returns>Admin entity.</returns>
        private Admin MapToEntity(AdminDto dto) =>
            new Admin
            {
                Id = dto.Id,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
                Name = dto.Name,
                CNP = dto.CNP,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = dto.CreatedAt
            };
    }
}
