using Microsoft.AspNetCore.Mvc;
using HMS.WebClient.Models;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HMS.WebClient.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public DoctorController(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IMedicalRecordRepository medicalRecordRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _medicalRecordRepository = medicalRecordRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Profile()
        {
            // Get the current doctor's ID from the session/claims
            var doctorId = 3; // TODO: Get from session/claims
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        public async Task<IActionResult> MedicalHistory()
        {
            // Get the current doctor's ID from the session/claims
            var doctorId = 3; // TODO: Get from session/claims
            
            var appointments = await _appointmentRepository.GetAllAsync();
            var doctorAppointments = appointments.Where(a => a.DoctorId == doctorId);
            
            return View(doctorAppointments);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(DoctorDto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var doctor = await _doctorRepository.GetByIdAsync(model.Id);
            if (doctor == null)
            {
                return NotFound();
            }

            var success = await _doctorRepository.UpdateAsync(doctor);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to update profile");
                return View("Profile", model);
            }

            return RedirectToAction(nameof(Profile));
        }
    }
} 