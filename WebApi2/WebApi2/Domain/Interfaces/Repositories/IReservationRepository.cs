using Domain.Entities;
using Domain.Interfaces.Models;

namespace Domain.Interfaces.Repositories;

public interface IReservationRepository
{
    IReadOnlyList<Reservation> GetReservations();
    Reservation? GetReservationForId( Guid id );
    void Save( Reservation reservation );
    void Update( Reservation reservation );
    void Delete( Guid id );
    int GetOverlappingReservationsCount( Guid roomTypeId, DateOnly arrival, DateOnly departure );
    IReadOnlyList<Reservation> GetFilteredReservations( Guid? propertyId, DateOnly? fromDate, DateOnly? toDate, string? guestName );
    bool HasReservations( Guid roomTypeId );
    IReadOnlyList<RoomTypeSearch> SearchAvailableOptions( string? city, DateOnly arrivalDate, DateOnly departureDate, int guests, decimal? maxPrice );
}
