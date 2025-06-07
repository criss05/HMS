using HMS.Shared.DTOs;
using HMS.WebClient.Attributes;
using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Enums;
using System;
using System.Threading.Tasks;
using AppointmentService = HMS.WebClient.Services.AppointmentService;
using DoctorService = HMS.WebClient.Services.DoctorService;
using AuthService = HMS.WebClient.Services.AuthService;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Patient)]
    public class AppointmentController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly DoctorService _doctorService;
        private readonly AuthService _authService;

        public AppointmentController(
            AppointmentService appointmentService,
            DoctorService doctorService,
            AuthService authService)
        {
            _appointmentService = appointmentService;
            _doctorService = doctorService;
            _authService = authService;
        }

        // View all patient's appointments
        public async Task<IActionResult> Index()
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var appointments = await _appointmentService.GetAppointmentsForPatientAsync(userId.Value);
            return View(appointments);
        }

        // Display form to book a new appointment
        public async Task<IActionResult> Book()
        {
            var allDoctors = await _doctorService.GetAllDoctorsAsync();
            ViewBag.Doctors = allDoctors;
            return View(new AppointmentDto { DateTime = DateTime.Now.AddDays(1) });
        }

        // Process the appointment booking
        [HttpPost]
        public async Task<IActionResult> Book(int? doctorId = null, int? procedureId = null)
        {
            var allDoctors = await _doctorService.GetAllDoctorsAsync();
            ViewBag.Doctors = allDoctors;

            var appointment = new AppointmentDto
            {
                DateTime = DateTime.Now.AddDays(1)
            };

            // Pre-select doctor if provided
            if (doctorId.HasValue && doctorId > 0)
            {
                appointment.DoctorId = doctorId.Value;

                // Load procedures for the selected doctor
                ViewBag.Procedures = await _appointmentService.GetProceduresForDoctorAsync(doctorId.Value);

                // Pre-select procedure if provided and valid
                if (procedureId.HasValue && procedureId > 0)
                {
                    var procedures = await _appointmentService.GetProceduresForDoctorAsync(doctorId.Value);
                    if (procedures.Any(p => p.Id == procedureId.Value))
                    {
                        appointment.ProcedureId = procedureId.Value;
                    }
                }
            }

            return View(appointment);
        }

        [HttpGet]
        public async Task<JsonResult> GetProceduresByDoctor(int doctorId)
        {
            if (doctorId <= 0)
                return Json(new List<object>());

            var procedures = await _appointmentService.GetProceduresForDoctorAsync(doctorId);

            // Format the procedures for the dropdown
            var formattedProcedures = procedures.Select(p => new {
                id = p.Id,
                name = p.Name
            });

            return Json(formattedProcedures);
        }

        // Cancel an appointment
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            try
            {
                var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
                if (appointment == null || appointment.PatientId != userId)
                {
                    TempData["ErrorMessage"] = "Appointment not found or access denied";
                    return RedirectToAction("Index");
                }

                var success = await _appointmentService.CancelAppointmentAsync(id);
                if (success)
                {
                    TempData["SuccessMessage"] = "Appointment canceled successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to cancel the appointment";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}