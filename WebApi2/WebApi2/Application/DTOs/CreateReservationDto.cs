using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateReservationDto
{
    [Required]
    public Guid PropertyId { get; set; }

    [Required]
    public Guid RoomTypeId { get; set; }

    [Required]
    public DateOnly ArrivalDate { get; set; }

    [Required]
    public DateOnly DepartureDate { get; set; }

    [Required]
    public TimeOnly ArrivalTime { get; set; } = new TimeOnly( 14, 0 );

    [Required]
    public TimeOnly DepartureTime { get; set; } = new TimeOnly( 12, 0 );

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [StringLength( ValidationConstants.MaxStringLength, ErrorMessage = "Строка не должна быть пустой!" )]
    public string GuestPhoneNumber { get; set; } = string.Empty;

    [Required]
    [Range( 1, ValidationConstants.MaxGuests, ErrorMessage = "Неверное количество гостей!" )]
    public int GuestCount { get; set; }
}