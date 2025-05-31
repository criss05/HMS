using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HMS.Shared.Enums;

namespace HMS.Shared.Entities
{
    public class Patient : User
    {
        [Required]
        public BloodType BloodType { get; set; }

        [Required]
        public string EmergencyContact { get; set; }

        [Required]
        public string Allergies { get; set; } = ""; // csv for now..

        [Required]
        public float Weight { get; set; }

        [Required]
        public float Height { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
