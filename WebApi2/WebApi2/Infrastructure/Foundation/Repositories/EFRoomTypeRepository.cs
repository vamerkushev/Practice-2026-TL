using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Exceptions;

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

    public RoomType? GetRoomTypeForId( Guid id )
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
        _dbContext.Update( roomType );
        _dbContext.SaveChanges();
    }

    public void Delete( Guid id )
    {
        RoomType? existingRoomType = GetRoomTypeForId( id );
        if ( existingRoomType == null )
        {
            throw new NotFoundException( $"RoomType с {id} ID не найден!" );
        }

        _dbContext.Set<RoomType>().Remove( existingRoomType );
        _dbContext.SaveChanges();
    }
}