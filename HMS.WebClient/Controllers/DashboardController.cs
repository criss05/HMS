using HMS.Shared.DTOs;
using HMS.WebClient.Attributes;
using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Enums;
using System.Threading.Tasks;
using HMS.WebClient.ViewModels;
using AppointmentService = HMS.WebClient.Services.AppointmentService;
using NotificationService = HMS.WebClient.Services.NotificationService;
using MedicalRecordService = HMS.WebClient.Services.MedicalRecordService;
using AuthService = HMS.WebClient.Services.AuthService;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Patient)]
    public class DashboardController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly MedicalRecordService _medicalRecordService;
        private readonly NotificationService _notificationService;
        private readonly AuthService _authService;

        public DashboardController(
            AppointmentService appointmentService,
            MedicalRecordService medicalRecordService,
            NotificationService notificationService,
            AuthService authService)
        {
            _appointmentService = appointmentService;
            _medicalRecordService = medicalRecordService;
            _notificationService = notificationService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var appointments = await _appointmentService.GetUpcomingAppointmentsForPatientAsync(userId.Value, 3);
            var medicalRecords = await _medicalRecordService.GetRecentMedicalRecordsForPatientAsync(userId.Value, 3);
            var notifications = await _notificationService.GetRecentUnreadNotificationsAsync(userId.Value, 5);

            var dashboardViewModel = new PatientDashboardViewModel
            {
                UpcomingAppointments = appointments,
                RecentMedicalRecords = medicalRecords,
                Notifications = notifications
            };

            return View(dashboardViewModel);
        }
    }
}