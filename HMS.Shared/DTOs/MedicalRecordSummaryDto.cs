using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMS.Shared.DTOs
{
    public class MedicalRecordSummaryDto
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;

        public int ProcedureId { get; set; }
        public string ProcedureName { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
