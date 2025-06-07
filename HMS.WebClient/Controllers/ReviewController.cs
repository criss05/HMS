using HMS.Shared.DTOs;
using HMS.WebClient.Attributes;
using Microsoft.AspNetCore.Mvc;
using HMS.Shared.Enums;
using System.Threading.Tasks;
using ReviewService = HMS.WebClient.Services.ReviewService;
using DoctorService = HMS.WebClient.Services.DoctorService;
using AuthService = HMS.WebClient.Services.AuthService;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Patient)]
    public class ReviewController : Controller
    {
        private readonly ReviewService _reviewService;
        private readonly DoctorService _doctorService;
        private readonly AuthService _authService;

        public ReviewController(
            ReviewService reviewService,
            DoctorService doctorService,
            AuthService authService)
        {
            _reviewService = reviewService;
            _doctorService = doctorService;
            _authService = authService;
        }

        // View form to submit a new review
        public async Task<IActionResult> Create(int doctorId)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
                return NotFound();

            ViewBag.Doctor = doctor;
            return View(new ReviewDto { DoctorId = doctorId });
        }

        // Process the review submission
        [HttpPost]
        public async Task<IActionResult> Create(ReviewDto review)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            review.PatientId = userId.Value;

            if (!ModelState.IsValid)
            {
                var doctor = await _doctorService.GetDoctorByIdAsync(review.DoctorId);
                ViewBag.Doctor = doctor;
                return View(review);
            }

            await _reviewService.CreateReviewAsync(review);
            TempData["SuccessMessage"] = "Review submitted successfully";
            return RedirectToAction("Details", "Doctor", new { id = review.DoctorId });
        }
    }
}