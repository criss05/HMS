using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftRepository _shiftRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public ShiftController(IShiftRepository shiftRepository, IDoctorRepository doctorRepository, IScheduleRepository scheduleRepository)
        {
            _shiftRepository = shiftRepository;
            _doctorRepository = doctorRepository;
            _scheduleRepository = scheduleRepository;
        }

        /// <summary>
        /// Retrieves all shifts.
        /// </summary>
        /// <returns>List of shifts.</returns>
        /// <response code="200">Returns the list of shifts.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Shift>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var shifts = await _shiftRepository.GetAllAsync();
            return Ok(shifts);
        }

        /// <summary>
        /// Retrieves a specific shift by id.
        /// </summary>
        /// <param name="id">Shift id.</param>
        /// <returns>The requested shift.</returns>
        /// <response code="200">Returns the requested shift.</response>
        /// <response code="404">If the shift is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Shift), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var shift = await _shiftRepository.GetByIdAsync(id);
            if (shift == null) return NotFound();
            return Ok(shift);
        }

        /// <summary>
        /// Creates a new shift with associated schedules.
        /// </summary>
        /// <param name="dto">Shift DTO with date, times, and doctor IDs.</param>
        /// <returns>The created shift.</returns>
        /// <response code="201">Returns the newly created shift.</response>
        /// <response code="400">If the input data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Shift), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ShiftDto dto)
        {
            // Validate doctor IDs exist
            var doctors = new List<Doctor>();
            foreach (var doctorId in dto.DoctorIds.Distinct())
            {
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                    return BadRequest($"Doctor with ID {doctorId} not found.");
                doctors.Add(doctor);
            }

            var shift = new Shift
            {
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
            };

            var createdShift = await _shiftRepository.AddAsync(shift);

            // Create schedules linking doctors to this shift
            foreach (var doctor in doctors)
            {
                var schedule = new Schedule
                {
                    DoctorId = doctor.Id,
                    ShiftId = createdShift.Id
                };
                await _scheduleRepository.AddAsync(schedule);
            }

            return CreatedAtAction(nameof(GetById), new { id = createdShift.Id }, createdShift);
        }

        /// <summary>
        /// Updates an existing shift and its associated schedules.
        /// </summary>
        /// <param name="id">Shift id.</param>
        /// <param name="dto">Shift DTO with updated data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Update was successful.</response>
        /// <response code="400">If input is invalid.</response>
        /// <response code="404">If shift not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ShiftDto dto)
        {
            var existingShift = await _shiftRepository.GetByIdAsync(id);
            if (existingShift == null) return NotFound();

            // Validate doctor IDs exist
            var doctors = new List<Doctor>();
            foreach (var doctorId in dto.DoctorIds.Distinct())
            {
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                if (doctor == null)
                    return BadRequest($"Doctor with ID {doctorId} not found.");
                doctors.Add(doctor);
            }

            // Update shift properties
            existingShift.Date = dto.Date;
            existingShift.StartTime = dto.StartTime;
            existingShift.EndTime = dto.EndTime;

            // Update schedules:
            // Remove existing schedules for this shift
            var existingSchedules = existingShift.Schedules.ToList();
            foreach (var sched in existingSchedules)
            {
                await _scheduleRepository.DeleteAsync(sched.DoctorId, sched.ShiftId);
            }

            // Add new schedules from DTO
            foreach (var doctor in doctors)
            {
                var newSchedule = new Schedule
                {
                    DoctorId = doctor.Id,
                    ShiftId = existingShift.Id
                };
                await _scheduleRepository.AddAsync(newSchedule);
            }

            await _shiftRepository.UpdateAsync(existingShift);
            return NoContent();
        }

        /// <summary>
        /// Deletes a shift.
        /// </summary>
        /// <param name="id">Shift id.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Deletion was successful.</response>
        /// <response code="404">If shift not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _shiftRepository.DeleteAsync(id);
            if (!result) return NotFound();

            // Optionally delete schedules linked to the shift as well

            return NoContent();
        }
    }
}
