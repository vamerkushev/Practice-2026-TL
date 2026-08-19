using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreatePropertyDto
{
    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string Country { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string Address { get; set; } = string.Empty;

    [Range( -90, 90, ErrorMessage = "Широта должна быть от -90 до 90 градусов!" )]
    public double Latitude { get; set; }

    [Range( -180, 180, ErrorMessage = "Долгота должна быть от -180 до 180 градусов!" )]
    public double Longitude { get; set; }
}