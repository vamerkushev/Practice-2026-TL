using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Foundation.Repositories;

public class EFReservationRepository : IReservationRepository
{
    private readonly HotelManagementDbContext _dbContext;

    public EFReservationRepository( HotelManagementDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Reservation> GetAllReservations()
    {
        return _dbContext.Set<Reservation>().ToList();
    }

    public Reservation? GetReservationById( Guid id )
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
        Reservation? existingReservation = GetReservationById( id );
        if ( existingReservation == null )
        {
            throw new KeyNotFoundException( $"Reservation с {id} ID не найден!" );
        }

        _dbContext.Set<Reservation>().Remove( existingReservation );
        _dbContext.SaveChanges();
    }

    public int GetOverlappingReservationsCount( Guid roomTypeId, DateTime arrival, DateTime departure )
    {
        return _dbContext.Set<Reservation>().Count( r =>
            r.RoomTypeId == roomTypeId && !r.IsCancelled &&
            r.ArrivalDate < departure &&
            r.DepartureDate > arrival );
    }

    public IReadOnlyList<Reservation> GetFilteredReservations( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName )
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
}