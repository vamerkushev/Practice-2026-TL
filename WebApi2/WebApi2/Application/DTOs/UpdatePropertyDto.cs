using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class UpdatePropertyDto
{
    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Поле не должно быть длиннее {1} символов!" )]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Поле не должно быть длиннее {1} символов!" )]
    public string Country { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Поле не должно быть длиннее {1} символов!" )]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Поле не должно быть длиннее {1} символов!" )]
    public string Address { get; set; } = string.Empty;

    [Range( -90, 90, ErrorMessage = "Широта должна быть от -90 до 90 градусов!" )]
    public double Latitude { get; set; }

    [Range( -180, 180, ErrorMessage = "Долгота должна быть от -180 до 180 градусов!" )]
    public double Longitude { get; set; }
}