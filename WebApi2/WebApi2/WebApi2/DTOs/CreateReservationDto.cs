namespace WebApi.DTOs;

public class CreateReservationDto
{
    public Guid PropertyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public string ArrivalTime { get; set; } = "14:00";
    public string DepartureTime { get; set; } = "12:00";
    public string GuestName { get; set; } = string.Empty;
    public string GuestPhoneNumber { get; set; } = string.Empty;
    public int GuestCount { get; set; }
}