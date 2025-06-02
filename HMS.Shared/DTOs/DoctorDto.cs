using System;
using System.Collections.Generic;
using HMS.Shared.Enums;

namespace HMS.Shared.DTOs
{
    public class DoctorDto
    {
        // User fields
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }  // enum type, adjust if you prefer string
        public string Name { get; set; } = null!;
        public string CNP { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        // Doctor-specific fields
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = "";

        public int YearsOfExperience { get; set; }
        public string LicenseNumber { get; set; } = "";

        public List<int> ScheduleIds { get; set; } = new();
        public List<int> ReviewIds { get; set; } = new();
        public List<int> AppointmentIds { get; set; } = new();
    }
}
