using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace WebApi2.Controllers;

[ApiController]
[Route( "api/properties" )]
public class PropertiesController : ControllerBase
{
    private readonly PropertyService _propertyService;

    public PropertiesController( PropertyService propertyService )
    {
        _propertyService = propertyService;
    }

    [HttpGet( "" )]
    public IActionResult GetProperties()
    {
        IReadOnlyList<Property> properties = _propertyService.GetProperties();

        IEnumerable<PropertyDto> result = properties.Select( PropertyDto.MapFromProperty );

        return Ok( result );
    }

    [HttpGet( "{id:guid}" )]
    public IActionResult GetProperty( [FromRoute] Guid id )
    {
        Property? property = _propertyService.GetPropertyForId( id );
        if ( property == null )
        {
            return NotFound();
        }

        PropertyDto result = new PropertyDto()
        {
            Id = property.Id,
            Name = property.Name,
            Country = property.Country,
            City = property.City,
            Address = property.Address,
            Latitude = property.Latitude,
            Longitude = property.Longitude
        };

        return Ok( result );
    }

    [HttpPost( "" )]
    public IActionResult CreateProperty( [FromBody] CreatePropertyDto request )
    {
        Guid propertyId = _propertyService.CreateProperty( request );
        return CreatedAtAction( nameof( GetProperty ), new { id = propertyId }, propertyId );
    }

    [HttpPut( "{id:guid}" )]
    public IActionResult UpdateProperty( [FromRoute] Guid id, [FromBody] UpdatePropertyDto request )
    {
        _propertyService.UpdateProperty( id, request );
        return NoContent();
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult DeleteProperty( [FromRoute] Guid id )
    {
        _propertyService.DeleteProperty( id );
        return NoContent();
    }
}