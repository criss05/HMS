using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Backend.Repositories.Interfaces;
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

        /// <summary>
        /// Retrieves all users from the system.
        /// </summary>
        /// <returns>List of all users.</returns>
        /// <response code="200">Returns the list of users.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), 200)]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _repository.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a specific user by unique id.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>The requested user.</returns>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">If user with specified id is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">User object to create.</param>
        /// <returns>Newly created user.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the user data is invalid or email is already taken.</response>
        [HttpPost]
        [ProducesResponseType(typeof(User), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check for existing email to avoid unique constraint violation
            var existingUserWithEmail = await _repository.GetByEmailAsync(user.Email);
            if (existingUserWithEmail != null)
                return BadRequest($"Email '{user.Email}' is already in use.");

            await _repository.AddAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">ID of the user to update.</param>
        /// <param name="user">Updated user object.</param>
        /// <response code="204">If update is successful (No Content).</response>
        /// <response code="400">If the input data is invalid, id mismatch, or email is already taken by another user.</response>
        /// <response code="404">If user with specified id is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
                return BadRequest("Id in URL and payload do not match");

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            // Check if the new email is already used by another user
            var userWithEmail = await _repository.GetByEmailAsync(user.Email);
            if (userWithEmail != null && userWithEmail.Id != id)
                return BadRequest($"Email '{user.Email}' is already in use by another user.");

            await _repository.UpdateAsync(user);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by id.
        /// </summary>
        /// <param name="id">ID of the user to delete.</param>
        /// <response code="204">If deletion is successful (No Content).</response>
        /// <response code="404">If user with specified id is not found.</response>
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
    }
}
