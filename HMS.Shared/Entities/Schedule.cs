using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Entities
{
    [Table("Schedules")]
    public class Schedule
    {
        [ForeignKey(nameof(Shift))]
        public int ShiftId { get; set; }
        public Shift Shift { get; set; } = null!;

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
