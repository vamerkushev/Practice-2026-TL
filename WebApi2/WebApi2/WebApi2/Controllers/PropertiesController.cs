using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route( "api/properties" )]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertiesController( IPropertyService propertyService )
    {
        _propertyService = propertyService;
    }

    [HttpGet( "" )]
    public IActionResult GetProperties()
    {
        IReadOnlyList<Property> properties = _propertyService.GetAllProperties();

        IEnumerable<PropertyDto> result = properties.Select( p => new PropertyDto
        {
            Id = p.Id,
            Name = p.Name,
            Country = p.Country,
            City = p.City,
            Address = p.Address,
            Latitude = p.Latitude,
            Longitude = p.Longitude
        } );

        return Ok( result );
    }

    [HttpGet( "{id:guid}" )]
    public IActionResult GetProperty( [FromRoute] Guid id )
    {
        Property? property = _propertyService.GetPropertyById( id );
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
        Property property = new Property( request.Name, request.Country, request.City, request.Address, request.Latitude, request.Longitude );
        _propertyService.CreateProperty( property );

        return Ok();
    }

    [HttpPut( "{id:guid}" )]
    public IActionResult UpdateProperty( [FromRoute] Guid id, [FromBody] UpdatePropertyDto request )
    {
        Property? property = _propertyService.GetPropertyById( id );
        if ( property == null )
        {
            return NotFound();
        }

        property.Update( request.Name, request.Country, request.City, request.Address, request.Latitude, request.Longitude );
        _propertyService.UpdateProperty( property );

        return Ok();
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult DeleteProperty( [FromRoute] Guid id )
    {
        try
        {
            _propertyService.DeleteProperty( id );
            return Ok();
        }
        catch ( KeyNotFoundException )
        {
            return NotFound();
        }
        catch ( InvalidOperationException e )
        {
            return BadRequest( e.Message );
        }
    }

    [HttpGet( "{propertyId:guid}/roomtypes" )]
    public IActionResult GetRoomTypes( [FromRoute] Guid propertyId )
    {
        IReadOnlyList<RoomType> roomTypes = _propertyService.GetRoomTypesByPropertyId( propertyId );

        IEnumerable<RoomTypeDto> result = roomTypes.Select( rt => new RoomTypeDto
        {
            Id = rt.Id,
            PropertyId = rt.PropertyId,
            Name = rt.Name,
            DailyPrice = rt.DailyPrice,
            Currency = rt.Currency,
            MinPersonCount = rt.MinPersonCount,
            MaxPersonCount = rt.MaxPersonCount,
            AvailableRoomsCount = rt.AvailableRoomsCount,
            Services = rt.Services,
            Amenities = rt.Amenities
        } );

        return Ok( result );
    }

    [HttpGet( "roomtypes/{id:guid}" )]
    public IActionResult GetRoomType( [FromRoute] Guid id )
    {
        RoomType? roomType = _propertyService.GetRoomTypeById( id );
        if ( roomType == null )
        {
            return NotFound();
        }

        RoomTypeDto result = new RoomTypeDto()
        {
            Id = roomType.Id,
            PropertyId = roomType.PropertyId,
            Name = roomType.Name,
            DailyPrice = roomType.DailyPrice,
            Currency = roomType.Currency,
            MinPersonCount = roomType.MinPersonCount,
            MaxPersonCount = roomType.MaxPersonCount,
            AvailableRoomsCount = roomType.AvailableRoomsCount,
            Services = roomType.Services,
            Amenities = roomType.Amenities
        };

        return Ok( result );
    }

    [HttpPost( "{propertyId:guid}/roomtypes" )]
    public IActionResult CreateRoomType( [FromRoute] Guid propertyId, [FromBody] CreateRoomTypeDto request )
    {
        RoomType roomType = new(
            propertyId,
            request.Name,
            request.DailyPrice,
            request.Currency,
            request.MinPersonCount,
            request.MaxPersonCount,
            request.AvailableRoomsCount,
            request.Services,
            request.Amenities
        );

        try
        {
            _propertyService.CreateRoomType( propertyId, roomType );
        }
        catch ( ArgumentException e )
        {
            return BadRequest( e.Message );
        }

        return Ok();
    }

    [HttpPut( "roomtypes/{id:guid}" )]
    public IActionResult UpdateRoomType( [FromRoute] Guid id, [FromBody] UpdateRoomTypeDto request )
    {
        RoomType? existing = _propertyService.GetRoomTypeById( id );
        if ( existing == null )
        {
            return NotFound();
        }

        existing.Update(
            request.Name,
            request.DailyPrice,
            request.Currency,
            request.MinPersonCount,
            request.MaxPersonCount,
            request.AvailableRoomsCount,
            request.Services,
            request.Amenities
        );
        _propertyService.UpdateRoomType( existing );

        return Ok();
    }

    [HttpDelete( "roomtypes/{id:guid}" )]
    public IActionResult DeleteRoomType( [FromRoute] Guid id )
    {
        try
        {
            _propertyService.DeleteRoomType( id );
        }
        catch ( InvalidOperationException e )
        {
            return BadRequest( e.Message );
        }
        catch ( KeyNotFoundException )
        {
            return NotFound();
        }

        return Ok();
    }
}