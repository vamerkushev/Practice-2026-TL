namespace WebApi.DTOs;

public class RoomTypeDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public int AvailableRoomsCount { get; set; }
    public List<string> Services { get; set; } = [];
    public List<string> Amenities { get; set; } = [];
}