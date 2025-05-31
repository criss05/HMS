using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Entities
{
    [Table("Appointments")]
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Patient))]
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        [ForeignKey(nameof(Doctor))]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        [ForeignKey(nameof(Procedure))]
        public int ProcedureId { get; set; }
        public Procedure Procedure { get; set; } = null!;

        [ForeignKey(nameof(Room))]
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        [Required]
        public DateTime DateTime { get; set; }
    }
}
