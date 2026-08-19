using Domain.Entities;
using Domain.Interfaces.Repositories;
using Application.DTOs;
using Domain.Exceptions;
using Domain.Interfaces.Models;

namespace Application.Services;

public class ReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;

    public ReservationService(
        IReservationRepository reservationRepository,
        IPropertyRepository propertyRepository,
        IRoomTypeRepository roomTypeRepository )
    {
        _reservationRepository = reservationRepository;
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
    }

    public IReadOnlyList<Reservation> GetReservations( Guid? propertyId, DateOnly? fromDate, DateOnly? toDate, string? guestName )
    {
        return _reservationRepository.GetFilteredReservations( propertyId, fromDate, toDate, guestName );
    }

    public Reservation? GetReservationForId( Guid id )
    {
        return _reservationRepository.GetReservationForId( id );
    }

    public Guid CreateReservation( CreateReservationDto reservationDto )
    {
        if ( reservationDto.ArrivalDate >= reservationDto.DepartureDate )
        {
            throw new BadRequestException( "Дата выезда должна быть позже даты заезда!" );
        }

        Property? property = _propertyRepository.GetPropertyForId( reservationDto.PropertyId );
        if ( property == null )
        {
            throw new NotFoundException( $"Property c {reservationDto.PropertyId} ID не найден!" );
        }

        RoomType? roomType = _roomTypeRepository.GetRoomTypeForId( reservationDto.RoomTypeId );
        if ( roomType == null )
        {
            throw new NotFoundException( $"RoomType c {reservationDto.RoomTypeId} ID не найдена!" );
        }

        if ( roomType.PropertyId != reservationDto.PropertyId )
        {
            throw new BadRequestException( "Тип номера не соответствует указанному отелю!" );
        }

        if ( reservationDto.GuestCount < roomType.MinPersonCount || reservationDto.GuestCount > roomType.MaxPersonCount )
        {
            throw new BadRequestException( "Количество гостей и мест в номере не совпадают!" );
        }

        int overlap = _reservationRepository.GetOverlappingReservationsCount( reservationDto.RoomTypeId, reservationDto.ArrivalDate, reservationDto.DepartureDate );

        if ( overlap >= roomType.AvailableRoomsCount )
        {
            throw new BadRequestException( "Нет свободных номеров!" );
        }

        int nights = reservationDto.DepartureDate.DayNumber - reservationDto.ArrivalDate.DayNumber;
        decimal totalPrice = roomType.DailyPrice * nights;

        Reservation reservation = new(
            reservationDto.PropertyId,
            reservationDto.RoomTypeId,
            reservationDto.ArrivalDate,
            reservationDto.DepartureDate,
            reservationDto.ArrivalTime,
            reservationDto.DepartureTime,
            reservationDto.GuestName,
            reservationDto.GuestPhoneNumber,
            reservationDto.GuestCount,
            totalPrice,
            roomType.Currency
        );

        _reservationRepository.Save( reservation );

        return reservation.Id;
    }

    public void CancelReservation( Guid id )
    {
        Reservation? reservation = _reservationRepository.GetReservationForId( id );
        if ( reservation == null )
        {
            throw new NotFoundException( "Бронирование не найдено!" );
        }

        if ( reservation.IsCancelled )
        {
            throw new BadRequestException( "Бронирование уже отменено!" );
        }

        reservation.Cancel();
        _reservationRepository.Update( reservation );
    }

    public IReadOnlyList<AvailableRoomType> SearchAvailable( string? city, DateOnly arrivalDate, DateOnly departureDate, int guests, decimal? maxPrice )
    {
        if ( arrivalDate >= departureDate )
        {
            throw new BadRequestException( "Дата заезда должна быть раньше даты выезда!" );
        }

        IReadOnlyList<RoomTypeSearch> result = _reservationRepository.SearchAvailableOptions( city, arrivalDate, departureDate, guests, maxPrice );

        return result.Select( r => new AvailableRoomType
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
        } ).ToList();
    }
}