namespace HMS.Shared.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int RoomId { get; set; }
        public int ProcedureId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
