using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;
using Application;

namespace WebApi2.Controllers;

[ApiController]
[Route( "api" )]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationsController( ReservationService reservationService )
    {
        _reservationService = reservationService;
    }

    [HttpGet( "search" )]
    public IActionResult Search( [FromQuery] SearchAvailabilityDto request )
    {
        IReadOnlyList<AvailableRoomType> domainResults = _reservationService.SearchAvailable(
            request.City,
            request.ArrivalDate,
            request.DepartureDate,
            request.Guests,
            request.MaxPrice );

        IEnumerable<SearchResultItemDto> result = domainResults.Select( SearchResultItemDto.MapFromAvailableRoomTypes );

        return Ok( result );
    }

    [HttpPost( "reservations" )]
    public IActionResult CreateReservation( [FromBody] CreateReservationDto request )
    {
        Guid reservationId = _reservationService.CreateReservation( request );
        return CreatedAtAction( nameof( GetReservation ), new { id = reservationId }, reservationId );
    }

    [HttpGet( "reservations" )]
    public IActionResult GetReservations(
        [FromQuery] Guid? propertyId,
        [FromQuery] DateOnly? fromDate,
        [FromQuery] DateOnly? toDate,
        [FromQuery] string? guestName )
    {
        IReadOnlyList<Reservation> reservations = _reservationService.GetReservations( propertyId, fromDate, toDate, guestName );

        IReadOnlyList<ReservationDto> result = reservations.Select( ReservationDto.MapFromReservation ).ToList();

        return Ok( result );
    }

    [HttpGet( "reservations/{id:guid}" )]
    public IActionResult GetReservation( [FromRoute] Guid id )
    {
        Reservation? reservation = _reservationService.GetReservationForId( id );
        if ( reservation == null )
        {
            return NotFound();
        }

        ReservationDto result = new ReservationDto
        {
            Id = reservation.Id,
            PropertyId = reservation.PropertyId,
            RoomTypeId = reservation.RoomTypeId,
            ArrivalDate = reservation.ArrivalDate,
            DepartureDate = reservation.DepartureDate,
            ArrivalTime = reservation.ArrivalTime,
            DepartureTime = reservation.DepartureTime,
            GuestName = reservation.GuestName,
            GuestPhoneNumber = reservation.GuestPhoneNumber,
            GuestCount = reservation.GuestCount,
            Total = reservation.Total,
            Currency = reservation.Currency,
            IsCancelled = reservation.IsCancelled
        };

        return Ok( result );
    }

    [HttpDelete( "reservations/{id:guid}" )]
    public IActionResult CancelReservation( [FromRoute] Guid id )
    {
        _reservationService.CancelReservation( id );
        return NotFound();
    }
}