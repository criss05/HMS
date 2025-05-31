using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.Entities
{
    [Table("Equipments")]
    public class Equipment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Specification { get; set; } = "";

        [Required]
        public string Type { get; set; } = null!;

        [Required]
        public int Stock { get; set; }
    }
}
