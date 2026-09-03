using Domain.Entities;

namespace Application.DTOs;

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

    public static RoomTypeDto MapFromRoomType( RoomType rt )
    {
        return new RoomTypeDto
        {
            Id = rt.Id,
            PropertyId = rt.PropertyId,
            Name = rt.Name,
            DailyPrice = rt.DailyPrice,
            Currency = rt.Currency,
            MinPersonCount = rt.MinPersonCount,
            MaxPersonCount = rt.MaxPersonCount,
            AvailableRoomsCount = rt.AvailableRoomsCount,
            Services = rt.Services,
            Amenities = rt.Amenities
        };
    }
}