using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Models;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Foundation.Repositories;

public class EFReservationRepository : IReservationRepository
{
    private readonly HotelManagementDbContext _dbContext;

    public EFReservationRepository( HotelManagementDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Reservation> GetReservations()
    {
        return _dbContext.Set<Reservation>().ToList();
    }

    public Reservation? GetReservationForId( Guid id )
    {
        return _dbContext.Set<Reservation>().Find( id );
    }

    public void Save( Reservation reservation )
    {
        _dbContext.Set<Reservation>().Add( reservation );
        _dbContext.SaveChanges();
    }

    public void Update( Reservation reservation )
    {
        _dbContext.Set<Reservation>().Update( reservation );
        _dbContext.SaveChanges();
    }

    public void Delete( Guid id )
    {
        Reservation? existingReservation = GetReservationForId( id );
        if ( existingReservation == null )
        {
            throw new NotFoundException( $"Reservation с {id} ID не найден!" );
        }

        _dbContext.Set<Reservation>().Remove( existingReservation );
        _dbContext.SaveChanges();
    }

    public int GetOverlappingReservationsCount( Guid roomTypeId, DateOnly arrival, DateOnly departure )
    {
        return _dbContext.Set<Reservation>().Count( r =>
            r.RoomTypeId == roomTypeId && !r.IsCancelled &&
            r.ArrivalDate < departure &&
            r.DepartureDate > arrival );
    }

    public IReadOnlyList<Reservation> GetFilteredReservations( Guid? propertyId, DateOnly? fromDate, DateOnly? toDate, string? guestName )
    {
        IQueryable<Reservation> query = _dbContext.Set<Reservation>().AsQueryable();

        if ( propertyId.HasValue )
        {
            query = query.Where( r => r.PropertyId == propertyId.Value );
        }

        if ( fromDate.HasValue )
        {
            query = query.Where( r => r.ArrivalDate >= fromDate.Value );
        }

        if ( toDate.HasValue )
        {
            query = query.Where( r => r.DepartureDate <= toDate.Value );
        }

        if ( !string.IsNullOrEmpty( guestName ) )
        {
            query = query.Where( r => r.GuestName.Contains( guestName ) );
        }

        return query.ToList();
    }

    public bool HasReservations( Guid roomTypeId )
    {
        return _dbContext.Set<Reservation>().Any( r => r.RoomTypeId == roomTypeId && !r.IsCancelled );
    }

    public IReadOnlyList<RoomTypeSearch> SearchAvailableOptions( string? city, DateOnly arrivalDate, DateOnly departureDate, int guests, decimal? maxPrice )
    {
        int nights = departureDate.DayNumber - arrivalDate.DayNumber;

        IQueryable<RoomTypeSearch> query = _dbContext.Set<Property>()
            .SelectMany( p => p.RoomTypes, ( p, rt ) => new { p, rt } )
            .Where( w => ( string.IsNullOrEmpty( city ) || w.p.City.Contains( city ) )
                && ( guests >= w.rt.MinPersonCount && guests <= w.rt.MaxPersonCount )
                && ( !maxPrice.HasValue || w.rt.DailyPrice <= maxPrice.Value ) )
            .Select( s => new
            {
                s.p,
                s.rt,
                overlaps = _dbContext.Set<Reservation>()
                    .Count( r => r.RoomTypeId == s.rt.Id
                    && !r.IsCancelled
                    && r.ArrivalDate < departureDate
                    && r.DepartureDate > arrivalDate )
            } )
            .Where( w => w.rt.AvailableRoomsCount - w.overlaps > 0 )
            .Select( s => new RoomTypeSearch
            {
                PropertyId = s.p.Id,
                PropertyName = s.p.Name,
                City = s.p.City,
                RoomTypeId = s.rt.Id,
                RoomTypeName = s.rt.Name,
                DailyPrice = s.rt.DailyPrice,
                Currency = s.rt.Currency,
                TotalForStay = s.rt.DailyPrice * nights,
                AvailableRooms = s.rt.AvailableRoomsCount - s.overlaps
            } );

        return query.ToList();
    }
}