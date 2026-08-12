using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route( "api" )]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController( IReservationService reservationService )
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

        IEnumerable<SearchResultItemDto> result = domainResults.Select( r => new SearchResultItemDto
        {
            PropertyId = r.PropertyId,
            PropertyName = r.PropertyName,
            City = r.City,
            RoomTypeId = r.RoomTypeId,
            RoomTypeName = r.RoomTypeName,
            DailyPrice = r.DailyPrice,
            Currency = r.Currency,
            TotalForStay = r.TotalForStay,
            AvailableRooms = r.AvailableRooms
        } );

        return Ok( result );
    }

    [HttpPost( "reservations" )]
    public IActionResult CreateReservation( [FromBody] CreateReservationDto request )
    {
        Reservation reservation = new(
            request.PropertyId,
            request.RoomTypeId,
            request.ArrivalDate,
            request.DepartureDate,
            request.ArrivalTime,
            request.DepartureTime,
            request.GuestName,
            request.GuestPhoneNumber,
            request.GuestCount,
            0,
            string.Empty
        );

        try
        {
            _reservationService.CreateReservation( reservation );
        }
        catch ( ArgumentException e )
        {
            return BadRequest( e.Message );
        }
        catch ( InvalidOperationException e )
        {
            return BadRequest( e.Message );
        }

        return Ok();
    }

    [HttpGet( "reservations" )]
    public IActionResult GetReservations(
        [FromQuery] Guid? propertyId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] string? guestName )
    {
        IReadOnlyList<Reservation> reservations = _reservationService.GetAllReservations( propertyId, fromDate, toDate, guestName );

        IReadOnlyList<ReservationDto> result = reservations.Select( r => new ReservationDto
        {
            Id = r.Id,
            PropertyId = r.PropertyId,
            RoomTypeId = r.RoomTypeId,
            ArrivalDate = r.ArrivalDate,
            DepartureDate = r.DepartureDate,
            ArrivalTime = r.ArrivalTime,
            DepartureTime = r.DepartureTime,
            GuestName = r.GuestName,
            GuestPhoneNumber = r.GuestPhoneNumber,
            GuestCount = r.GuestCount,
            Total = r.Total,
            Currency = r.Currency,
            IsCancelled = r.IsCancelled
        } ).ToList();

        return Ok( result );
    }

    [HttpGet( "reservations/{id:guid}" )]
    public IActionResult GetReservation( [FromRoute] Guid id )
    {
        Reservation? reservation = _reservationService.GetReservationById( id );
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
        try
        {
            _reservationService.CancelReservation( id );
        }
        catch ( KeyNotFoundException )
        {
            return NotFound();
        }
        catch ( InvalidOperationException e )
        {
            return BadRequest( e.Message );
        }

        return Ok();
    }
}