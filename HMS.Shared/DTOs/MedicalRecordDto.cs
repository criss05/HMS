namespace HMS.Shared.DTOs
{
    public class MedicalRecordDto
    {
        public int? Id { get; set; }  // nullable for create (null) or update (with value)
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int ProcedureId { get; set; }
        public string Diagnosis { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }  // optional: let server set on create
    }
}
