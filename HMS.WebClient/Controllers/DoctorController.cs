using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.DTOs;
using System.Threading.Tasks;
using System;

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
            try
            {
                // Get the current doctor's ID from the session/claims
                var doctorId = 3; // TODO: Get from session/claims
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                
                return View(doctor);
            }
            catch (KeyNotFoundException)
            {
                // Doctor not found
                return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in Profile action: {ex.Message}");
                
                // Add error message to be displayed to the user
                ModelState.AddModelError("", "An error occurred while loading the profile. Please try again later.");
                
                // Return to view with error message
                return View();
            }
        }

        public async Task<IActionResult> MedicalHistory()
        {
            try
            {
                // Get the current doctor's ID from the session/claims
                var doctorId = 3; // TODO: Get from session/claims
                var doctor = await _doctorRepository.GetByIdAsync(doctorId);
                
                return View(doctor.Appointments);
            }
            catch (KeyNotFoundException)
            {
                // Doctor not found
                return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in MedicalHistory action: {ex.Message}");
                
                // Add error message to be displayed to the user
                ModelState.AddModelError("", "An error occurred while loading the medical history. Please try again later.");
                
                // Return to view with error message
                return View(Array.Empty<HMS.Shared.Entities.Appointment>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(DoctorDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Profile", model);
                }

                var doctor = await _doctorRepository.GetByIdAsync(model.Id);
                if (doctor == null)
                {
                    return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");
                }

                var success = await _doctorRepository.UpdateAsync(doctor);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update profile. Please try again.");
                    return View("Profile", model);
                }

                // Add success message
                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in UpdateProfile action: {ex.Message}");
                
                // Add error message to be displayed to the user
                ModelState.AddModelError("", "An error occurred while updating the profile. Please try again later.");
                
                // Return to view with error message
                return View("Profile", model);
            }
        }
    }
} 