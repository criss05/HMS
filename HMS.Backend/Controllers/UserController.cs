using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HMS.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _repository.GetAllAsync();

            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add(MapToDto(user));
            }

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(MapToDto(user));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUserWithEmail = await _repository.GetByEmailAsync(dto.Email);
            if (existingUserWithEmail != null)
                return BadRequest($"Email '{dto.Email}' is already in use.");

            var user = MapToEntity(dto);
            await _repository.AddAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, MapToDto(user));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Id in URL and payload do not match");

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            var userWithEmail = await _repository.GetByEmailAsync(dto.Email);
            if (userWithEmail != null && userWithEmail.Id != id)
                return BadRequest($"Email '{dto.Email}' is already in use by another user.");

            // Update fields on existing entity from dto
            existingUser.Email = dto.Email;
            existingUser.Password = dto.Password;  // Be careful: hashing should be handled here or in service layer
            existingUser.Role = dto.Role;
            existingUser.Name = dto.Name;
            existingUser.CNP = dto.CNP;
            existingUser.PhoneNumber = dto.PhoneNumber;
            existingUser.CreatedAt = dto.CreatedAt;

            await _repository.UpdateAsync(existingUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        private UserDto MapToDto(User user) =>
            new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Name = user.Name,
                CNP = user.CNP,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt
            };

        private User MapToEntity(UserDto dto) =>
            new User
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
