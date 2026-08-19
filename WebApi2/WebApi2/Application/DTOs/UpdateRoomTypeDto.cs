using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class UpdateRoomTypeDto
{
    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range( 0, ValidationConstants.MaxPrice, ErrorMessage = "Cтоимость должна быть положительной!" )]
    public decimal DailyPrice { get; set; }

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string Currency { get; set; } = "RUB";

    [Required]
    [Range( 1, ValidationConstants.MaxGuests, ErrorMessage = "Неверное количество гостей!" )]
    public int MinPersonCount { get; set; }

    [Required]
    [Range( 1, ValidationConstants.MaxGuests, ErrorMessage = "Неверное количество гостей!" )]
    public int MaxPersonCount { get; set; }

    [Required]
    public int AvailableRoomsCount { get; set; }

    public List<string> Services { get; set; } = [];
    public List<string> Amenities { get; set; } = [];
}