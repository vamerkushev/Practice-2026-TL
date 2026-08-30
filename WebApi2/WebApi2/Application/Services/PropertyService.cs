using Domain.Entities;
using Domain.Interfaces.Repositories;
using Application.DTOs;
using Domain.Exceptions;

namespace Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;

    public PropertyService( IPropertyRepository propertyRepository, IRoomTypeRepository roomTypeRepository )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
    }

    public IReadOnlyList<Property> GetProperties()
    {
        return _propertyRepository.GetProperties();
    }

    public Property? GetById( Guid id )
    {
        return _propertyRepository.GetById( id );
    }

    public Guid CreateProperty( CreatePropertyDto propertyDto )
    {
        Property property = new Property( propertyDto.Name, propertyDto.Country, propertyDto.City, propertyDto.Address, propertyDto.Latitude, propertyDto.Longitude );
        _propertyRepository.Save( property );
        return property.Id;
    }

    public void UpdateProperty( Guid id, UpdatePropertyDto propertyDto )
    {
        Property? property = _propertyRepository.GetById( id );
        if ( property == null )
        {
            throw new NotFoundException( $"Property c {id} ID не найден!" );
        }

        property.Update( propertyDto.Name, propertyDto.Country, propertyDto.City, propertyDto.Address, propertyDto.Latitude, propertyDto.Longitude );
        _propertyRepository.Update( property );
    }

    public void DeleteProperty( Guid id )
    {
        Property? property = _propertyRepository.GetById( id );
        if ( property == null )
        {
            throw new NotFoundException( $"Property с {id} ID не найден!" );
        }

        IReadOnlyList<RoomType> roomTypes = _roomTypeRepository.GetByPropertyId( id );
        if ( roomTypes.Any() )
        {
            throw new BadRequestException( "Нельзя удалить отель! Сначала удалите все типы номеров!" );
        }

        _propertyRepository.Delete( id );
    }
}