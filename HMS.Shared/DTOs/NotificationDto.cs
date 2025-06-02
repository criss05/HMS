using System;

namespace HMS.Shared.DTOs
{
    public class NotificationDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; } = null!;

        public DateTime DeliveryDateTime { get; set; }
    }
}
