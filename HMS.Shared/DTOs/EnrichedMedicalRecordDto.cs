using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.DTOs
{
    public class EnrichedMedicalRecordDto
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProcedureName { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
