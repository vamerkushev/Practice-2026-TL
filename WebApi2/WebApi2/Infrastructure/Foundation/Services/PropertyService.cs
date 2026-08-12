using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepo;
    private readonly IRoomTypeRepository _roomTypeRepo;

    public PropertyService( IPropertyRepository propRepo, IRoomTypeRepository roomRepo )
    {
        _propertyRepo = propRepo;
        _roomTypeRepo = roomRepo;
    }

    public IReadOnlyList<Property> GetAllProperties()
    {
        return _propertyRepo.GetAllProperty();
    }

    public Property? GetPropertyById( Guid id )
    {
        return _propertyRepo.GetPropertyById( id );
    }

    public void CreateProperty( Property property )
    {
        _propertyRepo.Save( property );
    }

    public void UpdateProperty( Property property )
    {
        _propertyRepo.Update( property );
    }

    public void DeleteProperty( Guid id )
    {
        Property? property = _propertyRepo.GetPropertyById( id );
        if ( property == null )
        {
            throw new KeyNotFoundException( $"Property с ID {id} не найден!" );
        }

        IReadOnlyList<RoomType> roomTypes = _roomTypeRepo.GetByPropertyId( id );
        if ( roomTypes.Any() )
        {
            throw new InvalidOperationException( "Нельзя удалить отель! Сначала удалите все типы номеров!" );
        }

        _propertyRepo.Delete( id );
    }

    public IReadOnlyList<RoomType> GetRoomTypesByPropertyId( Guid propertyId )
    {
        return _roomTypeRepo.GetByPropertyId( propertyId );
    }

    public RoomType? GetRoomTypeById( Guid id )
    {
        return _roomTypeRepo.GetRoomTypeById( id );
    }

    public void CreateRoomType( Guid propertyId, RoomType roomType )
    {
        if ( _propertyRepo.GetPropertyById( propertyId ) == null )
        {
            throw new ArgumentException( $"Property с {propertyId} ID  не найдена!" );
        }

        _roomTypeRepo.Save( roomType );
    }

    public void UpdateRoomType( RoomType roomType )
    {
        _roomTypeRepo.Update( roomType );
    }

    public void DeleteRoomType( Guid id )
    {
        if ( _roomTypeRepo.HasReservations( id ) )
        {
            throw new InvalidOperationException( "Нельзя удалить тип комнаты с активными бронированиями!" );
        }

        _roomTypeRepo.Delete( id );
    }
}