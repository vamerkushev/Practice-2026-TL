using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IReservationService
{
    IReadOnlyList<Reservation> GetAllReservations( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName );
    Reservation? GetReservationById( Guid id );
    void CreateReservation( Reservation reservation );
    void CancelReservation( Guid id );
    IReadOnlyList<AvailableRoomType> SearchAvailable( string? city, DateTime arrivalDate, DateTime departureDate, int guests, decimal? maxPrice );
}