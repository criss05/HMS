using HMS.WebClient.Services;
using HMS.Shared.Enums;
using HMS.WebClient.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HMS.WebClient.Controllers
{
    [Authorize(UserRole.Patient)]
    public class NotificationController : Controller
    {
        private readonly NotificationService _notificationService;
        private readonly AuthService _authService;

        public NotificationController(
            NotificationService notificationService,
            AuthService authService)
        {
            _notificationService = notificationService;
            _authService = authService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var notifications = await _notificationService.GetNotificationsForUserAsync(userId.Value);
            return View(notifications);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Account");

            await _notificationService.MarkNotificationAsReadAsync(id, userId.Value);
            return RedirectToAction("Index");
        }
    }
}