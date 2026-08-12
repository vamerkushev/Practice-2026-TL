using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPropertyRepository
{
    IReadOnlyList<Property> GetAllProperty();
    Property? GetPropertyById( Guid id );
    void Save( Property property );
    void Update( Property property );
    void Delete( Guid id );
}