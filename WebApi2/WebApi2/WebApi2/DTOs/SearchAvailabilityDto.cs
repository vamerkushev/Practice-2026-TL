namespace WebApi.DTOs;

public class SearchAvailabilityDto
{
    public string? City { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public int Guests { get; set; }
    public decimal? MaxPrice { get; set; }
}