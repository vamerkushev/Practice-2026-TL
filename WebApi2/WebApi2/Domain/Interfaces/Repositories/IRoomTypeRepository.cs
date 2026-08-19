using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRoomTypeRepository
{
    IReadOnlyList<RoomType> GetByPropertyId( Guid propertyId );
    RoomType? GetRoomTypeForId( Guid id );
    void Save( RoomType roomType );
    void Update( RoomType roomType );
    void Delete( Guid id );
}
