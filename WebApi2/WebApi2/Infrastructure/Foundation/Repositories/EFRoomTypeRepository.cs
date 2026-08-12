using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Foundation.Repositories;

public class EFRoomTypeRepository : IRoomTypeRepository
{
    private readonly HotelManagementDbContext _dbContext;

    public EFRoomTypeRepository( HotelManagementDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<RoomType> GetByPropertyId( Guid propertyId )
    {
        return _dbContext.Set<RoomType>().Where( r => r.PropertyId == propertyId ).ToList();
    }

    public RoomType? GetRoomTypeById( Guid id )
    {
        return _dbContext.Set<RoomType>().Find( id );
    }

    public void Save( RoomType roomType )
    {
        _dbContext.Set<RoomType>().Add( roomType );
        _dbContext.SaveChanges();
    }

    public void Update( RoomType roomType )
    {
        RoomType? existingRoomType = GetRoomTypeById( roomType.Id );
        if ( existingRoomType == null )
        {
            throw new KeyNotFoundException( $"RoomType с {roomType.Id} ID не найден!" );
        }

        existingRoomType.CopyFrom( roomType );
        _dbContext.SaveChanges();
    }

    public void Delete( Guid id )
    {
        RoomType? existingRoomType = GetRoomTypeById( id );
        if ( existingRoomType == null )
        {
            throw new KeyNotFoundException( $"RoomType с {id} ID не найден!" );
        }

        _dbContext.Set<RoomType>().Remove( existingRoomType );
        _dbContext.SaveChanges();
    }

    public bool HasReservations( Guid roomTypeId )
    {
        return _dbContext.Set<Reservation>().Any( r => r.RoomTypeId == roomTypeId );
    }
}