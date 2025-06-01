namespace HMS.Shared.DTOs
{
    public class DepartmentDto
    {
        public string Name { get; set; } = string.Empty;

        // Optional: for read operations
        public List<int>? DoctorIds { get; set; }
    }
}
