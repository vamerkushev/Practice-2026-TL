using Domain.Entities;

namespace Application.DTOs;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public TimeOnly DepartureTime { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string GuestPhoneNumber { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; } = string.Empty;
    public bool IsCancelled { get; set; }

    public static ReservationDto MapFromReservation( Reservation r )
    {
        return new ReservationDto
        {
            Id = r.Id,
            PropertyId = r.PropertyId,
            RoomTypeId = r.RoomTypeId,
            ArrivalDate = r.ArrivalDate,
            DepartureDate = r.DepartureDate,
            ArrivalTime = r.ArrivalTime,
            DepartureTime = r.DepartureTime,
            GuestName = r.GuestName,
            GuestPhoneNumber = r.GuestPhoneNumber,
            GuestCount = r.GuestCount,
            Total = r.Total,
            Currency = r.Currency,
            IsCancelled = r.IsCancelled
        };
    }
}