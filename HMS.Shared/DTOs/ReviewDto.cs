namespace HMS.Shared.DTOs
{
    public class ReviewDto
    {
        public int? Id { get; set; }  // Optional: nullable to allow create operations without specifying Id
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Value { get; set; }
    }
}
