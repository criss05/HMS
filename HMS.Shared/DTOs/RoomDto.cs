    namespace HMS.Shared.DTOs
    {
        public class RoomDto
        {
            public int Id { get; set; }
            public int DepartmentId { get; set; }
            public int Capacity { get; set; }
            public List<int> EquipmentIds { get; set; } = new();
            public List<int> AppointmentIds { get; set; } = new();
        }
    }