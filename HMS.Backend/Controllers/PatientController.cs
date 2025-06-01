using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public PatientController(
            IPatientRepository patientRepository,
            IReviewRepository reviewRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository)
        {
            _patientRepository = patientRepository;
            _reviewRepository = reviewRepository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }

        /// <summary>
        /// Gets all patients.
        /// </summary>
        /// <returns>List of patients.</returns>
        /// <response code="200">Returns all patients.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Patient>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientRepository.GetAllAsync();
            return Ok(patients);
        }

        /// <summary>
        /// Gets a specific patient by ID.
        /// </summary>
        /// <param name="id">Patient's ID.</param>
        /// <returns>The requested patient.</returns>
        /// <response code="200">Returns the patient.</response>
        /// <response code="404">If patient not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Patient), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        /// <summary>
        /// Creates a new patient.
        /// </summary>
        /// <param name="dto">Patient DTO containing patient data.</param>
        /// <returns>The created patient.</returns>
        /// <response code="201">Returns the newly created patient.</response>
        /// <response code="400">If input data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Patient), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] PatientDto dto)
        {
            var reviews = new List<Review>();
            foreach (var reviewId in dto.ReviewIds)
            {
                var review = await _reviewRepository.GetByIdAsync(reviewId);
                if (review == null)
                    return BadRequest($"Review with ID {reviewId} not found.");
                reviews.Add(review);
            }

            var appointments = new List<Appointment>();
            foreach (var appointmentId in dto.AppointmentIds)
            {
                var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
                if (appointment == null)
                    return BadRequest($"Appointment with ID {appointmentId} not found.");
                appointments.Add(appointment);
            }

            var medicalRecords = new List<MedicalRecord>();
            foreach (var medicalRecordId in dto.MedicalRecordIds)
            {
                var medicalRecord = await _medicalRecordRepository.GetByIdAsync(medicalRecordId);
                if (medicalRecord == null)
                    return BadRequest($"MedicalRecord with ID {medicalRecordId} not found.");
                medicalRecords.Add(medicalRecord);
            }

            var patient = new Patient
            {
                BloodType = Enum.Parse<HMS.Shared.Enums.BloodType>(dto.BloodType),
                EmergencyContact = dto.EmergencyContact,
                Allergies = dto.Allergies,
                Weight = dto.Weight,
                Height = dto.Height,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Reviews = reviews,
                Appointments = appointments,
                MedicalRecords = medicalRecords
            };

            var createdPatient = await _patientRepository.AddAsync(patient);
            return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
        }

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">Patient's ID.</param>
        /// <param name="dto">Patient DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Successfully updated.</response>
        /// <response code="400">If input data is invalid.</response>
        /// <response code="404">If patient not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto dto)
        {
            var existing = await _patientRepository.GetByI_
