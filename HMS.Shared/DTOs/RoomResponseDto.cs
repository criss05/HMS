using System.Text.Json.Serialization;

namespace HMS.Shared.DTOs;

public class RoomResponseDto
{
    [JsonPropertyName("$values")]
    public List<RoomDto> Values { get; set; } = new List<RoomDto>();
}