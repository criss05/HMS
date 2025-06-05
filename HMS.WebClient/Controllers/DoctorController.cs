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
using System.Text.Json;
using Microsoft.Extensions.Logging;
using HMS.Shared.DTOs.Patient;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Doctor)]
    public class DoctorController : Controller
    {
        private readonly DoctorService _doctorService;
        private readonly IMedicalRecordRepository _medicalRecordRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly AuthService _authService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(
            DoctorService doctorService,
            IMedicalRecordRepository medicalRecordRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            AuthService authService,
            ILogger<DoctorController> logger)
        {
            _doctorService = doctorService;
            _medicalRecordRepository = medicalRecordRepository;
            _appointmentRepository = appointmentRepository;
            _patientRepository = patientRepository;
            _authService = authService;
            _logger = logger;
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
        public async Task<IActionResult> Edit(DoctorViewModel doctorViewModel, string scheduleIdsJson, string reviewIdsJson, string appointmentIdsJson)
        {
            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null || currentUser.Id != doctorViewModel.Id)
            {
                return Forbid();
            }

            try
            {
                var existingDoctor = await _doctorService.GetDoctorByIdAsync(doctorViewModel.Id);
                if (existingDoctor == null)
                {
                    ModelState.AddModelError("", "Doctor not found. Please try again.");
                    return View(doctorViewModel);
                }

                doctorViewModel.DepartmentId = existingDoctor.DepartmentId;
                doctorViewModel.DepartmentName = existingDoctor.DepartmentName;

                doctorViewModel.ScheduleIds = JsonSerializer.Deserialize<List<int>>(scheduleIdsJson) ?? new List<int>();
                doctorViewModel.ReviewIds = JsonSerializer.Deserialize<List<int>>(reviewIdsJson) ?? new List<int>();
                doctorViewModel.AppointmentIds = JsonSerializer.Deserialize<List<int>>(appointmentIdsJson) ?? new List<int>();

                ModelState.Clear();
                if (!TryValidateModel(doctorViewModel))
                {
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                    return View(doctorViewModel);
                }

                var success = await _doctorService.UpdateDoctorAsync(doctorViewModel);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed to update doctor profile. Please try again.");
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
                    _logger.LogWarning("MedicalHistory: No current user found");
                    return RedirectToAction("Login", "Account");
                }

                _logger.LogInformation($"MedicalHistory: Fetching doctor profile for ID {currentUser.Id}");
                var doctorViewModel = await _doctorService.GetDoctorByIdAsync(currentUser.Id);
                if (doctorViewModel == null)
                {
                    _logger.LogWarning($"MedicalHistory: Doctor profile not found for ID {currentUser.Id}");
                    TempData["ErrorMessage"] = "Doctor profile not found.";
                    return RedirectToAction(nameof(Profile));
                }

                // Get medical records
                _logger.LogInformation($"MedicalHistory: Fetching medical records for doctor ID {currentUser.Id}");
                var records = await _medicalRecordRepository.GetAllAsync();
                if (records == null)
                {
                    _logger.LogWarning("MedicalHistory: GetAllAsync returned null for medical records");
                    records = new List<MedicalRecordDto>();
                }
                var doctorRecords = records.Where(r => r.DoctorId == currentUser.Id).ToList();

                // Get upcoming appointments
                _logger.LogInformation($"MedicalHistory: Fetching appointments for doctor ID {currentUser.Id}");
                var allAppointments = await _appointmentRepository.GetAllAsync();
                if (allAppointments == null)
                {
                    _logger.LogWarning("MedicalHistory: GetAllAsync returned null for appointments");
                    allAppointments = new List<AppointmentDto>();
                }
                var upcomingAppointments = allAppointments
                    .Where(a => a.DoctorId == currentUser.Id && a.DateTime > DateTime.Now)
                    .OrderBy(a => a.DateTime)
                    .ToList();

                // Get patient names for upcoming appointments
                var appointmentsWithPatients = new List<AppointmentDto>();
                foreach (var appointment in upcomingAppointments)
                {
                    var patient = await _patientRepository.GetByIdAsync(appointment.PatientId);
                    if (patient != null)
                    {
                        ViewData[$"PatientName_{appointment.PatientId}"] = patient.Name;
                    }
                    appointmentsWithPatients.Add(appointment);
                }

                // Get patient names for recent patients
                var recentPatients = doctorRecords
                    .GroupBy(r => new { r.PatientId })
                    .Select(async g => {
                        var patient = await _patientRepository.GetByIdAsync(g.Key.PatientId);
                        return new PatientSummaryViewModel
                        {
                            Id = g.Key.PatientId,
                            Name = patient?.Name ?? $"Patient {g.Key.PatientId}",
                            LastVisit = g.Max(r => r.CreatedAt ?? DateTime.MinValue),
                            VisitCount = g.Count()
                        };
                    })
                    .ToList();

                var resolvedRecentPatients = await Task.WhenAll(recentPatients);

                _logger.LogInformation($"MedicalHistory: Creating view model with {doctorRecords.Count} records and {appointmentsWithPatients.Count} appointments");

                // Create view model with all sections
                var viewModel = new DoctorMedicalHistoryViewModel
                {
                    DoctorName = doctorViewModel.Name,
                    DepartmentName = doctorViewModel.DepartmentName,
                    MedicalRecords = doctorRecords.OrderByDescending(r => r.CreatedAt).ToList(),
                    RecentPatients = resolvedRecentPatients.OrderByDescending(p => p.LastVisit).ToList(),
                    UpcomingAppointments = appointmentsWithPatients
                };

                _logger.LogInformation("MedicalHistory: Successfully created view model");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in MedicalHistory action");
                TempData["ErrorMessage"] = $"An error occurred while loading the medical history: {ex.Message}";
                return RedirectToAction(nameof(Profile));
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

                // Get patient name
                var patient = await _patientRepository.GetByIdAsync(record.PatientId);
                if (patient != null)
                {
                    ViewData["PatientName"] = patient.Name;
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