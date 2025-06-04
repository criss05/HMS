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
                return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while loading the profile. Please try again later.");
                return View(new Doctor());
            }
        }

        public async Task<IActionResult> MedicalHistory()
        {
            try
            {
                // Get the current doctor's ID from the session/claims
                var doctorId = 3; // TODO: Get from session/claims
                var records = await _medicalRecordRepository.GetAllAsync();
                var doctorRecords = records?.Where(r => r.DoctorId == doctorId).ToList();
                
                return View(doctorRecords ?? new List<MedicalRecord>());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while loading the medical history. Please try again later.");
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

                TempData["SuccessMessage"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the profile. Please try again later.");
                return View("Profile", model);
            }
        }
    }
} 