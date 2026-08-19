using Domain.Entities;
using Domain.Interfaces.Repositories;
using Application.DTOs;
using Domain.Exceptions;

namespace Application.Services;

public class RoomTypeService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;

    public RoomTypeService( IPropertyRepository propertyRepository, IRoomTypeRepository roomTypeRepository, IReservationRepository reservationRepository )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
    }

    public IReadOnlyList<RoomType> GetRoomTypesForProperty( Guid propertyId )
    {
        return _roomTypeRepository.GetByPropertyId( propertyId );
    }

    public RoomType? GetRoomTypeForId( Guid id )
    {
        return _roomTypeRepository.GetRoomTypeForId( id );
    }

    public Guid CreateRoomType( Guid propertyId, CreateRoomTypeDto roomTypeDto )
    {
        if ( roomTypeDto.MinPersonCount >= roomTypeDto.MaxPersonCount )
        {
            throw new BadRequestException( "Минимальное количество гостей должно быть меньше максимального!" );
        }

        if ( _propertyRepository.GetPropertyForId( propertyId ) == null )
        {
            throw new NotFoundException( $"Property с {propertyId} ID  не найдена!" );
        }

        RoomType roomType = new(
            propertyId,
            roomTypeDto.Name,
            roomTypeDto.DailyPrice,
            roomTypeDto.Currency,
            roomTypeDto.MinPersonCount,
            roomTypeDto.MaxPersonCount,
            roomTypeDto.AvailableRoomsCount,
            roomTypeDto.Services,
            roomTypeDto.Amenities
        );

        _roomTypeRepository.Save( roomType );

        return roomType.Id;
    }

    public void UpdateRoomType( Guid id, UpdateRoomTypeDto roomTypeDto )
    {
        if ( roomTypeDto.MinPersonCount >= roomTypeDto.MaxPersonCount )
        {
            throw new BadRequestException( "Минимальное количество гостей должно быть меньше максимального!" );
        }

        RoomType? roomType = _roomTypeRepository.GetRoomTypeForId( id );
        if ( roomType == null )
        {
            throw new NotFoundException( $"RoomType c {id} ID не найден!" );
        }

        roomType.Update(
            roomTypeDto.Name,
            roomTypeDto.DailyPrice,
            roomTypeDto.Currency,
            roomTypeDto.MinPersonCount,
            roomTypeDto.MaxPersonCount,
            roomTypeDto.AvailableRoomsCount,
            roomTypeDto.Services,
            roomTypeDto.Amenities
        );
    }

    public void DeleteRoomType( Guid id )
    {
        if ( _reservationRepository.HasReservations( id ) )
        {
            throw new BadRequestException( "Нельзя удалить тип комнаты с активными бронированиями!" );
        }

        _roomTypeRepository.Delete( id );
    }
}