using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        /// <summary>
        /// Retrieves all doctors.
        /// </summary>
        /// <returns>List of doctors.</returns>
        /// <response code="200">Returns the list of doctors.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Doctor>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return Ok(doctors);
        }

        /// <summary>
        /// Retrieves a specific doctor by id.
        /// </summary>
        /// <param name="id">Doctor's id.</param>
        /// <returns>The requested doctor.</returns>
        /// <response code="200">Returns the requested doctor.</response>
        /// <response code="404">If the doctor is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Doctor), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        /// <summary>
        /// Creates a new doctor.
        /// </summary>
        /// <param name="doctor">Doctor object to create.</param>
        /// <returns>The created doctor.</returns>
        /// <response code="201">Returns the newly created doctor.</response>
        /// <response code="400">If the doctor object is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Doctor), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDoctor = await _doctorRepository.AddAsync(doctor);
            return CreatedAtAction(nameof(GetById), new { id = createdDoctor.Id }, createdDoctor);
        }

        /// <summary>
        /// Updates an existing doctor.
        /// </summary>
        /// <param name="id">Doctor's id.</param>
        /// <param name="doctor">Doctor object with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input is invalid.</response>
        /// <response code="404">If doctor not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.Id)
                return BadRequest("Doctor ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _doctorRepository.UpdateAsync(doctor);
            if (!result) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a doctor.
        /// </summary>
        /// <param name="id">Doctor's id.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If doctor not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _doctorRepository.DeleteAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
