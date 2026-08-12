using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IPropertyService
{
    IReadOnlyList<Property> GetAllProperties();
    Property? GetPropertyById( Guid id );
    void CreateProperty( Property property );
    void UpdateProperty( Property property );
    void DeleteProperty( Guid id );

    IReadOnlyList<RoomType> GetRoomTypesByPropertyId( Guid propertyId );
    RoomType? GetRoomTypeById( Guid id );
    void CreateRoomType( Guid propertyId, RoomType roomType );
    void UpdateRoomType( RoomType roomType );
    void DeleteRoomType( Guid id );
}