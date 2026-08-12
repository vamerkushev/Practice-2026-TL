using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IReservationRepository
{
    IReadOnlyList<Reservation> GetAllReservations();
    Reservation? GetReservationById( Guid id );
    void Save( Reservation reservation );
    void Update( Reservation reservation );
    void Delete( Guid id );
    int GetOverlappingReservationsCount( Guid roomTypeId, DateTime arrival, DateTime departure );
    IReadOnlyList<Reservation> GetFilteredReservations( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName );
}
