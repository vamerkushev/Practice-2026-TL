using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPropertyRepository
{
    IReadOnlyList<Property> GetProperties();
    Property? GetById( Guid id );
    void Save( Property property );
    void Update( Property property );
    void Delete( Guid id );
}