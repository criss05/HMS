using HMS.Backend.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using HMS.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IDepartmentRepository _departmentRepository;

        public DoctorController(IDoctorRepository doctorRepository, IDepartmentRepository departmentRepository)
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Doctor>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorRepository.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Doctor), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Doctor), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] DoctorDto dto)
        {
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                return BadRequest($"Department with ID {dto.DepartmentId} not found.");

            var doctor = new Doctor
            {
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role,
                Name = dto.Name,
                CNP = dto.CNP,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = dto.CreatedAt, // Usually set by the system, adjust if needed

                DepartmentId = dto.DepartmentId,
                Department = department,
                YearsOfExperience = dto.YearsOfExperience,
                LicenseNumber = dto.LicenseNumber,

                // You might want to initialize collections here if needed, or in the entity constructor
            };

            var createdDoctor = await _doctorRepository.AddAsync(doctor);
            return CreatedAtAction(nameof(GetById), new { id = createdDoctor.Id }, createdDoctor);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] DoctorDto dto)
        {
            var existing = await _doctorRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                return BadRequest($"Department with ID {dto.DepartmentId} not found.");

            // Update user fields
            existing.Email = dto.Email;
            existing.Password = dto.Password;
            existing.Role = dto.Role;
            existing.Name = dto.Name;
            existing.CNP = dto.CNP;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.CreatedAt = dto.CreatedAt; // Usually not updated manually

            // Update doctor fields
            existing.DepartmentId = dto.DepartmentId;
            existing.Department = department;
            existing.YearsOfExperience = dto.YearsOfExperience;
            existing.LicenseNumber = dto.LicenseNumber;

            var success = await _doctorRepository.UpdateAsync(existing);
            if (!success)
                return BadRequest("Failed to update the doctor.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
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
