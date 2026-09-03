using Domain.Entities;

namespace Application.DTOs;

public class PropertyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static PropertyDto MapFromProperty( Property p )
    {
        return new PropertyDto
        {
            Id = p.Id,
            Name = p.Name,
            Country = p.Country,
            City = p.City,
            Address = p.Address,
            Latitude = p.Latitude,
            Longitude = p.Longitude
        };
    }
}