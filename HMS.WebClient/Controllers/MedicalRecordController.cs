using HMS.Shared.DTOs;
using HMS.WebClient.Attributes;
using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Enums;
using System;
using System.Threading.Tasks;
using System.Linq;
using MedicalRecordService = HMS.WebClient.Services.MedicalRecordService;
using AuthService = HMS.WebClient.Services.AuthService;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Patient)]
    public class MedicalRecordController : Controller
    {
        private readonly MedicalRecordService _medicalRecordService;
        private readonly AuthService _authService;

        public MedicalRecordController(
            MedicalRecordService medicalRecordService,
            AuthService authService)
        {
            _medicalRecordService = medicalRecordService;
            _authService = authService;
        }

        // View all patient's medical records
        public async Task<IActionResult> Index()
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var records = await _medicalRecordService.GetMedicalRecordsForPatientAsync(userId.Value);
            return View(records);
        }

        // View details of a specific medical record
        public async Task<IActionResult> Details(int id)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var record = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
            if (record == null || record.PatientId != userId)
                return NotFound();

            return View(record);
        }

        // Download medical record as PDF
        public async Task<IActionResult> Download(int id)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var record = await _medicalRecordService.GetMedicalRecordByIdAsync(id);
            if (record == null || record.PatientId != userId)
                return NotFound();

            // Generate PDF (implement PDF generation service)
            var pdfBytes = await _medicalRecordService.GenerateRecordPdfAsync(record);

            return File(pdfBytes, "application/pdf", $"MedicalRecord_{id}.pdf");
        }
    }
}