namespace Application.DTOs;

public class SearchResultItemDto
{
    public Guid PropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public Guid RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal TotalForStay { get; set; }
    public int AvailableRooms { get; set; }

    public static SearchResultItemDto MapFromAvailableRoomTypes( AvailableRoomType art )
    {
        return new SearchResultItemDto
        {
            PropertyId = art.PropertyId,
            PropertyName = art.PropertyName,
            City = art.City,
            RoomTypeId = art.RoomTypeId,
            RoomTypeName = art.RoomTypeName,
            DailyPrice = art.DailyPrice,
            Currency = art.Currency,
            TotalForStay = art.TotalForStay,
            AvailableRooms = art.AvailableRooms
        };
    }
}