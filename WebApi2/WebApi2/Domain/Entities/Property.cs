namespace Domain.Entities;

public class Property
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public string Country { get; private set; }
    public string City { get; private set; }
    public string Address { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }

    public ICollection<RoomType> RoomTypes { get; private set; } = new List<RoomType>();

    private Property()
    {
    }

    public Property( string name, string country, string city, string address, double latitude, double longitude )
    {
        Id = Guid.NewGuid();
        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void Update( string name, string country, string city, string address, double latitude, double longitude )
    {
        Name = name;
        Country = country;
        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public void CopyFrom( Property other )
    {
        Update( other.Name, other.Country, other.City, other.Address, other.Latitude, other.Longitude );
    }
}