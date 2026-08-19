using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Foundation.Repositories;

public class EFPropertyRepository : IPropertyRepository
{
    private readonly HotelManagementDbContext _dbContext;

    public EFPropertyRepository( HotelManagementDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Property> GetProperty()
    {
        return _dbContext.Set<Property>().ToList();
    }

    public Property? GetPropertyForId( Guid id )
    {
        return _dbContext.Set<Property>().Find( id );
    }

    public void Save( Property property )
    {
        _dbContext.Set<Property>().Add( property );
        _dbContext.SaveChanges();
    }

    public void Update( Property property )
    {
        _dbContext.Update( property );
        _dbContext.SaveChanges();
    }

    public void Delete( Guid id )
    {
        Property? existingProperty = GetPropertyForId( id );
        if ( existingProperty == null )
        {
            throw new NotFoundException( $"Property с {id} ID не найден!" );
        }

        _dbContext.Set<Property>().Remove( existingProperty );
        _dbContext.SaveChanges();
    }
}
