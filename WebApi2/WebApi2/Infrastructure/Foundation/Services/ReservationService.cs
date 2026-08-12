using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepo;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IRoomTypeRepository _roomTypeRepo;

    public ReservationService(
        IReservationRepository reservationRepo,
        IPropertyRepository propertyRepo,
        IRoomTypeRepository roomTypeRepo )
    {
        _reservationRepo = reservationRepo;
        _propertyRepo = propertyRepo;
        _roomTypeRepo = roomTypeRepo;
    }

    public IReadOnlyList<Reservation> GetAllReservations( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName )
    {
        return _reservationRepo.GetFilteredReservations( propertyId, fromDate, toDate, guestName );
    }

    public Reservation? GetReservationById( Guid id )
    {
        return _reservationRepo.GetReservationById( id );
    }

    public void CreateReservation( Reservation reservation )
    {
        if ( reservation.ArrivalDate >= reservation.DepartureDate )
        {
            throw new ArgumentException( "Дата выезда должна быть позже даты заезда!" );
        }

        RoomType? roomType = _roomTypeRepo.GetRoomTypeById( reservation.RoomTypeId );
        if ( roomType == null )
        {
            throw new ArgumentException( $"RoomType c {reservation.RoomTypeId} ID не найдена!" );
        }

        if ( reservation.GuestCount < roomType.MinPersonCount || reservation.GuestCount > roomType.MaxPersonCount )
        {
            throw new ArgumentException( "Количество гостей и мест в номере не совпадают!" );
        }

        int overlap = _reservationRepo.GetOverlappingReservationsCount( reservation.RoomTypeId, reservation.ArrivalDate, reservation.DepartureDate );

        if ( overlap >= roomType.AvailableRoomsCount )
        {
            throw new InvalidOperationException( "Нет свободных номеров!" );
        }

        int nights = ( reservation.DepartureDate - reservation.ArrivalDate ).Days;
        reservation.SetTotalPrice( roomType.DailyPrice * nights, roomType.Currency );

        _reservationRepo.Save( reservation );
    }

    public void CancelReservation( Guid id )
    {
        Reservation? reservation = _reservationRepo.GetReservationById( id );
        if ( reservation == null )
        {
            throw new KeyNotFoundException( "Бронирование не найдено!" );
        }

        if ( reservation.IsCancelled )
        {
            throw new InvalidOperationException( "Бронирование уже отменено!" );
        }

        reservation.Cancel();
        _reservationRepo.Update( reservation );
    }

    public IReadOnlyList<AvailableRoomType> SearchAvailable( string? city, DateTime arrivalDate, DateTime departureDate, int guests, decimal? maxPrice )
    {
        List<AvailableRoomType> results = new List<AvailableRoomType>();
        IReadOnlyList<Property> properties = _propertyRepo.GetAllProperty();

        if ( !string.IsNullOrEmpty( city ) )
        {
            properties = properties.Where( p => p.City.Contains( city, StringComparison.OrdinalIgnoreCase ) ).ToList();
        }

        foreach ( Property? property in properties )
        {
            IReadOnlyList<RoomType> roomTypes = _roomTypeRepo.GetByPropertyId( property.Id );
            foreach ( RoomType? roomType in roomTypes )
            {
                if ( guests < roomType.MinPersonCount || guests > roomType.MaxPersonCount )
                    continue;

                if ( maxPrice.HasValue && roomType.DailyPrice > maxPrice.Value )
                    continue;

                int overlap = _reservationRepo.GetOverlappingReservationsCount( roomType.Id, arrivalDate, departureDate );
                int available = roomType.AvailableRoomsCount - overlap;

                if ( available <= 0 )
                    continue;

                int nights = ( departureDate - arrivalDate ).Days;
                results.Add( new AvailableRoomType
                {
                    PropertyId = property.Id,
                    PropertyName = property.Name,
                    City = property.City,
                    RoomTypeId = roomType.Id,
                    RoomTypeName = roomType.Name,
                    DailyPrice = roomType.DailyPrice,
                    Currency = roomType.Currency,
                    TotalForStay = roomType.DailyPrice * nights,
                    AvailableRooms = available
                } );
            }
        }

        return results;
    }
}