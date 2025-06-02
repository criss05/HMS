using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public ReviewController(IReviewRepository reviewRepository, IPatientRepository patientRepository, IDoctorRepository doctorRepository)
        {
            _reviewRepository = reviewRepository;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
        }

        /// <summary>
        /// Get all reviews.
        /// </summary>
        /// <returns>A list of reviews.</returns>
        /// <response code="200">Returns the list of reviews</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Review>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return Ok(reviews);
        }

        /// <summary>
        /// Get review by ID.
        /// </summary>
        /// <param name="id">The ID of the review.</param>
        /// <returns>The requested review.</returns>
        /// <response code="200">Returns the review</response>
        /// <response code="404">If the review is not found</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Review), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            return Ok(review);
        }

        /// <summary>
        /// Create new review.
        /// </summary>
        /// <param name="dto">The review data transfer object.</param>
        /// <returns>The created review.</returns>
        /// <response code="201">Returns the newly created review</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Review), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ReviewDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                return BadRequest($"Patient with ID {dto.PatientId} not found.");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            var review = new Review
            {
                PatientId = dto.PatientId,
                Patient = patient,
                DoctorId = dto.DoctorId,
                Doctor = doctor,
                Value = dto.Value
            };

            var createdReview = await _reviewRepository.AddAsync(review);
            return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview);
        }

        /// <summary>
        /// Update existing review.
        /// </summary>
        /// <param name="id">The ID of the review to update.</param>
        /// <param name="dto">The updated review data.</param>
        /// <response code="204">Successfully updated</response>
        /// <response code="400">If input data is invalid</response>
        /// <response code="404">If the review is not found</response>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewDto dto)
        {
            var existingReview = await _reviewRepository.GetByIdAsync(id);
            if (existingReview == null)
                return NotFound();

            var patient = await _patientRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                return BadRequest($"Patient with ID {dto.PatientId} not found.");

            var doctor = await _doctorRepository.GetByIdAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest($"Doctor with ID {dto.DoctorId} not found.");

            existingReview.PatientId = dto.PatientId;
            existingReview.Patient = patient;
            existingReview.DoctorId = dto.DoctorId;
            existingReview.Doctor = doctor;
            existingReview.Value = dto.Value;

            await _reviewRepository.UpdateAsync(existingReview);
            return NoContent();
        }

        /// <summary>
        /// Delete review by ID.
        /// </summary>
        /// <param name="id">The ID of the review to delete.</param>
        /// <response code="204">Successfully deleted</response>
        /// <response code="404">If the review is not found</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _reviewRepository.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
