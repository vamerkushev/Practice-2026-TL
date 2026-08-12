using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Foundation.Repositories;

public class EFPropertyRepository : IPropertyRepository
{
    private readonly HotelManagementDbContext _dbContext;

    public EFPropertyRepository( HotelManagementDbContext dbContext )
    {
        _dbContext = dbContext;
    }

    public IReadOnlyList<Property> GetAllProperty()
    {
        return _dbContext.Set<Property>().ToList();
    }

    public Property? GetPropertyById( Guid id )
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
        Property? existingProperty = GetPropertyById( property.Id );
        if ( existingProperty == null )
        {
            throw new KeyNotFoundException( $"Property с {property.Id} ID не найден!" );
        }

        existingProperty.CopyFrom( property );
        _dbContext.SaveChanges();
    }

    public void Delete( Guid id )
    {
        Property? existingProperty = GetPropertyById( id );
        if ( existingProperty == null )
        {
            throw new KeyNotFoundException( $"Property с {id} ID не найден!" );
        }

        _dbContext.Set<Property>().Remove( existingProperty );
        _dbContext.SaveChanges();
    }
}
