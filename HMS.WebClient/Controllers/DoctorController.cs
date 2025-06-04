using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.DTOs;
using System.Threading.Tasks;

namespace HMS.WebClient.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Profile()
        {
            // Get the current doctor's ID from the session/claims
            var doctorId = 1; // TODO: Get from session/claims
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
            var doctorId = 1; // TODO: Get from session/claims
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            
            if (doctor == null)
            {
                return NotFound();
            }

            // For now, we'll just show the doctor's appointments from their entity
            return View(doctor.Appointments);
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