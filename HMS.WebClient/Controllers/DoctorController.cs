using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Repositories.Interfaces;
using HMS.Shared.DTOs;
using HMS.Shared.Entities;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HMS.WebClient.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public DoctorController(
            IDoctorRepository doctorRepository,
            IMedicalRecordRepository medicalRecordRepository)
        {
            _doctorRepository = doctorRepository;
            _medicalRecordRepository = medicalRecordRepository;
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
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner exception stack trace: {ex.InnerException.StackTrace}");
                }
                
                // Add error message to be displayed to the user
                ModelState.AddModelError("", "An error occurred while loading the profile. Please try again later.");
                
                // Return to view with error message
                return View(new Doctor()); // Return empty doctor to avoid null reference
            }
        }

        public async Task<IActionResult> MedicalHistory()
        {
            try
            {
                // Get the current doctor's ID from the session/claims
                var doctorId = 3; // TODO: Get from session/claims
                var records = await _medicalRecordRepository.GetAllAsync();
                
                // Log the records for debugging
                Console.WriteLine($"Retrieved {records?.Count() ?? 0} records");
                foreach (var record in records ?? Enumerable.Empty<MedicalRecord>())
                {
                    Console.WriteLine($"Record ID: {record.Id}");
                    Console.WriteLine($"Patient: {record.Patient?.Name ?? "null"}");
                    Console.WriteLine($"Doctor: {record.Doctor?.Name ?? "null"}");
                    Console.WriteLine($"Procedure: {record.Procedure?.Name ?? "null"}");
                    Console.WriteLine("---");
                }
                
                var doctorRecords = records?.Where(r => r.DoctorId == doctorId).ToList();
                Console.WriteLine($"Filtered to {doctorRecords?.Count ?? 0} records for doctor {doctorId}");
                
                return View(doctorRecords ?? new List<MedicalRecord>());
            }
            catch (Exception ex)
            {
                // Log the error in detail
                Console.WriteLine($"Error in MedicalHistory action: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner exception stack trace: {ex.InnerException.StackTrace}");
                }
                
                // Add error message to be displayed to the user
                ModelState.AddModelError("", "An error occurred while loading the medical history. Please try again later.");
                
                // Return to view with error message
                return View(new List<MedicalRecord>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(Doctor model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Profile", model);
                }

                var existingDoctor = await _doctorRepository.GetByIdAsync(model.Id);
                if (existingDoctor == null)
                {
                    return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");
                }

                // Update only the editable fields
                existingDoctor.Name = model.Name;
                existingDoctor.Email = model.Email;
                existingDoctor.PhoneNumber = model.PhoneNumber;
                existingDoctor.YearsOfExperience = model.YearsOfExperience;
                existingDoctor.LicenseNumber = model.LicenseNumber;
                existingDoctor.CNP = model.CNP;
                
                // Keep the existing password
                existingDoctor.Password = existingDoctor.Password;

                var success = await _doctorRepository.UpdateAsync(existingDoctor);
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
                // Log the error in detail
                Console.WriteLine($"Error in UpdateProfile action: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner exception stack trace: {ex.InnerException.StackTrace}");
                }
                
                // Add error message to be displayed to the user
                ModelState.AddModelError("", "An error occurred while updating the profile. Please try again later.");
                
                // Return to view with error message
                return View("Profile", model);
            }
        }
    }
} 