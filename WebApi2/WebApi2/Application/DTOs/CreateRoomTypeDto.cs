using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateRoomTypeDto
{
    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Поле не должно быть длиннее {1} символов!" )]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range( 1, ValidationConstants.MaxPrice, ErrorMessage = "Cтоимость должна быть положительной!" )]
    public decimal DailyPrice { get; set; }

    [Required]
    [StringLength( ValidationConstants.CurrencyTitleLength, MinimumLength = ValidationConstants.CurrencyTitleLength, ErrorMessage = "Введите короткое название валюты, состоящее из 3 букв!" )]
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