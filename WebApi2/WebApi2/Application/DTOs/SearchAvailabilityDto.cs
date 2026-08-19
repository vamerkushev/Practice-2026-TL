using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class SearchAvailabilityDto
{
    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string? City { get; set; }

    [Required]
    public DateOnly ArrivalDate { get; set; }

    [Required]
    public DateOnly DepartureDate { get; set; }

    [Required]
    [Range( 1, ValidationConstants.MaxGuests, ErrorMessage = "Неверное количество гостей!" )]
    public int Guests { get; set; }

    [Range( 0, ValidationConstants.MaxPrice, ErrorMessage = "Максимальная стоимость должна быть положительной!" )]
    public decimal? MaxPrice { get; set; }
}