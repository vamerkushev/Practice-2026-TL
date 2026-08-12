namespace Domain.Entities;

public class RoomType
{
    public Guid Id { get; private set; }
    public Guid PropertyId { get; private set; }
    public string Name { get; private set; }
    public decimal DailyPrice { get; private set; }
    public string Currency { get; private set; }
    public int MinPersonCount { get; private set; }
    public int MaxPersonCount { get; private set; }
    public int AvailableRoomsCount { get; private set; }
    public List<string> Services { get; private set; } = [];
    public List<string> Amenities { get; private set; } = [];

    public Property? Property { get; private set; }

    private RoomType()
    {
    }

    public RoomType(
        Guid propertyId,
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        int availableRoomsCount,
        List<string>? services = null,
        List<string>? amenities = null )
    {
        Id = Guid.NewGuid();
        PropertyId = propertyId;
        Name = name;
        DailyPrice = dailyPrice;
        Currency = currency;
        MinPersonCount = minPersonCount;
        MaxPersonCount = maxPersonCount;
        AvailableRoomsCount = availableRoomsCount;
        Services = services ?? [];
        Amenities = amenities ?? [];
    }

    public void Update(
        string name,
        decimal dailyPrice,
        string currency,
        int minPersonCount,
        int maxPersonCount,
        int availableRoomsCount,
        List<string>? services = null,
        List<string>? amenities = null )
    {
        Name = name;
        DailyPrice = dailyPrice;
        Currency = currency;
        MinPersonCount = minPersonCount;
        MaxPersonCount = maxPersonCount;
        AvailableRoomsCount = availableRoomsCount;
        Services = services ?? Services;
        Amenities = amenities ?? Amenities;
    }

    public void CopyFrom( RoomType other )
    {
        Update( other.Name, other.DailyPrice, other.Currency, other.MinPersonCount, other.MaxPersonCount, other.AvailableRoomsCount, other.Services, other.Amenities );
    }
}
