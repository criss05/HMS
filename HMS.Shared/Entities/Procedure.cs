using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Entities
{
    [Table("Procedures")]
    public class Procedure
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; } // max 24h // lets hope this doesnt fuck things up
    }
}
