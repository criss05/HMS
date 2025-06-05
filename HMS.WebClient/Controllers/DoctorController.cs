using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Repositories.Interfaces;
using HMS.WebClient.Services;
using HMS.WebClient.ViewModels;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using HMS.Shared.DTOs;
using HMS.WebClient.Attributes;
using HMS.Shared.Enums;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Doctor)]
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly AuthService _authService;

        public DoctorController(
            DoctorService doctorService,
            IMedicalRecordRepository medicalRecordRepository,
            AuthService authService)
        {
            _doctorService = doctorService;
            _medicalRecordRepository = medicalRecordRepository;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Profile));
        }

        public async Task<IActionResult> Profile()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var doctorViewModel = await _doctorService.GetDoctorByIdAsync(currentUser.Id);
                
                if (doctorViewModel == null)
                {
                    return NotFound("Doctor not found. Please make sure you are logged in with a valid doctor account.");
                }

                return View(doctorViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while loading the profile. Please try again later.");
                return View(new DoctorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Id != id)
            {
                return Forbid();
            }

            var doctorViewModel = await _doctorService.GetDoctorByIdAsync(id);
            if (doctorViewModel == null)
            {
                return NotFound();
            }

            return View(doctorViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DoctorViewModel doctorViewModel)
        {
            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Id != doctorViewModel.Id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    ModelState.AddModelError("", error.ErrorMessage);
                }
                return View(doctorViewModel);
            }

            try
            {
                var success = await _doctorService.UpdateDoctorAsync(doctorViewModel);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update doctor profile. Please try again.");
                    return View(doctorViewModel);
                }

                // Get the updated doctor data
                var updatedDoctor = await _doctorService.GetDoctorByIdAsync(doctorViewModel.Id);
                if (updatedDoctor == null)
                {
                    ModelState.AddModelError("", "Failed to retrieve updated profile. Please try again.");
                    return View(doctorViewModel);
                }

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the profile. Please try again later.");
                return View(doctorViewModel);
            }
        }

        public async Task<IActionResult> MedicalHistory()
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var records = await _medicalRecordRepository.GetAllAsync();
                var doctorRecords = records?.Where(r => r.DoctorId == currentUser.Id).ToList();
                
                return View(doctorRecords ?? new List<MedicalRecordDto>());
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while loading the medical history. Please try again later.");
                return View(new List<MedicalRecordDto>());
            }
        }

        public async Task<IActionResult> ViewRecord(int id)
        {
            try
            {
                var currentUser = _authService.GetCurrentUser();
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var records = await _medicalRecordRepository.GetAllAsync();
                var record = records?.FirstOrDefault(r => r.Id == id && r.DoctorId == currentUser.Id);

                if (record == null)
                {
                    return NotFound("Medical record not found or you don't have permission to view it.");
                }

                return View(record);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while loading the medical record. Please try again later.");
                return RedirectToAction(nameof(MedicalHistory));
            }
        }
    }
} 