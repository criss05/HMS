using System.Collections.Generic;

namespace HMS.Shared.DTOs
{
    public class ShiftDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public List<int> DoctorIds { get; set; } = new List<int>();
    }
}
