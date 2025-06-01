using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMS.Shared.Entities;
using HMS.Backend.Repositories.Interfaces;

namespace HMS.Backend.Controllers
{
    /// <summary>
    /// API controller for handling operations related to patients.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        /// <summary>
        /// Constructor that injects the patient repository.
        /// </summary>
        public PatientsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>List of all patients.</returns>
        /// <response code="200">Returns the list of patients</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Patient>), 200)]
        public async Task<IEnumerable<Patient>> GetAll()
        {
            return await _patientRepository.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a patient by their ID.
        /// </summary>
        /// <param name="id">The ID of the patient.</param>
        /// <returns>The patient if found.</returns>
        /// <response code="200">Returns the patient</response>
        /// <response code="404">If the patient is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Patient), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Patient>> GetById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return patient;
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="patient">The patient to create.</param>
        /// <returns>The newly created patient.</returns>
        /// <response code="201">Returns the created patient</response>
        [HttpPost]
        [ProducesResponseType(typeof(Patient), 201)]
        public async Task<IActionResult> Create(Patient patient)
        {
            await _patientRepository.AddAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">The ID of the patient to update.</param>
        /// <param name="patient">The updated patient data.</param>
        /// <response code="204">Update was successful</response>
        /// <response code="400">If the ID in the URL does not match the patient ID</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, Patient patient)
        {
            if (id != patient.Id) return BadRequest();
            await _patientRepository.UpdateAsync(patient);
            return NoContent();
        }

        /// <summary>
        /// Deletes a patient by ID.
        /// </summary>
        /// <param name="id">The ID of the patient to delete.</param>
        /// <response code="204">Delete was successful</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
