using System.Collections.Generic;

namespace HMS.Shared.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = "";

        public int YearsOfExperience { get; set; }
        public string LicenseNumber { get; set; } = "";

        public List<int> ScheduleIds { get; set; } = new();
        public List<int> ReviewIds { get; set; } = new();
        public List<int> AppointmentIds { get; set; } = new();
    }
}
