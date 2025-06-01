using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Entities
{
    [Table("Shifts")]
    public class Shift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateOnly Date { get; set; }  // Use DateOnly for date without time (C# 10+)

        [Required]
        public TimeOnly StartTime { get; set; }  // Use TimeOnly for time (C# 10+)

        [Required]
        public TimeOnly EndTime { get; set; }

        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
