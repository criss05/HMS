namespace HMS.Shared.DTOs
{
    public class ProcedureDto
    {
        public int Id { get; set; }  // Include Id for updates and retrievals
        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;
        public TimeSpan Duration { get; set; }
    }
}
