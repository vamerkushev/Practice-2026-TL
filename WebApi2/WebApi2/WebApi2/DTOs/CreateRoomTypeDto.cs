namespace WebApi.DTOs;

public class CreateRoomTypeDto
{
    public string Name { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = "RUB";
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public int AvailableRoomsCount { get; set; }
    public List<string> Services { get; set; } = [];
    public List<string> Amenities { get; set; } = [];
}