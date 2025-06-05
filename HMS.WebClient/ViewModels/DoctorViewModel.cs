using HMS.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HMS.WebClient.ViewModels
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "CNP")]
        public string CNP { get; set; } = null!;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Department")]
        public string DepartmentName { get; set; } = "";

        [Required]
        [Display(Name = "Years of Experience")]
        [Range(0, 100)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; } = "";

        public List<int> ScheduleIds { get; set; } = new();
        public List<int> ReviewIds { get; set; } = new();
        public List<int> AppointmentIds { get; set; } = new();

        [Display(Name = "Number of Schedules")]
        public int SchedulesCount => ScheduleIds.Count;

        [Display(Name = "Number of Reviews")]
        public int ReviewsCount => ReviewIds.Count;

        [Display(Name = "Number of Appointments")]
        public int AppointmentsCount => AppointmentIds.Count;
    }
} 