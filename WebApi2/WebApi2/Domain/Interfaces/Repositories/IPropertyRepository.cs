using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPropertyRepository
{
    IReadOnlyList<Property> GetProperty();
    Property? GetPropertyForId( Guid id );
    void Save( Property property );
    void Update( Property property );
    void Delete( Guid id );
}