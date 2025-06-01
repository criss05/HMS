namespace HMS.Shared.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specification { get; set; }
        public string Type { get; set; }
        public int Stock { get; set; }
        public List<int> RoomIds { get; set; }
    }
}