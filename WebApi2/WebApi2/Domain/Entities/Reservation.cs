namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; private init; }
    public Guid PropertyId { get; private set; }
    public Guid RoomTypeId { get; private set; }
    public DateTime ArrivalDate { get; private set; }
    public DateTime DepartureDate { get; private set; }
    public string ArrivalTime { get; private set; }
    public string DepartureTime { get; private set; }
    public string GuestName { get; private set; }
    public string GuestPhoneNumber { get; private set; }
    public int GuestCount { get; private set; }
    public decimal Total { get; private set; }
    public string Currency { get; private set; }
    public bool IsCancelled { get; private set; }

    public RoomType? RoomType { get; private set; }
    public Property? Property { get; private set; }

    private Reservation()
    {
    }

    public Reservation(
        Guid propertyId,
        Guid roomTypeId,
        DateTime arrivalDate,
        DateTime departureDate,
        string arrivalTime,
        string departureTime,
        string guestName,
        string guestPhoneNumber,
        int guestCount,
        decimal total,
        string currency
        )
    {
        Id = Guid.NewGuid();
        PropertyId = propertyId;
        RoomTypeId = roomTypeId;
        ArrivalDate = arrivalDate;
        DepartureDate = departureDate;
        ArrivalTime = arrivalTime;
        DepartureTime = departureTime;
        GuestName = guestName;
        GuestPhoneNumber = guestPhoneNumber;
        GuestCount = guestCount;
        Total = total;
        Currency = currency;
        IsCancelled = false;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    public void SetTotalPrice( decimal total, string currency )
    {
        Total = total;
        Currency = currency;
    }
}