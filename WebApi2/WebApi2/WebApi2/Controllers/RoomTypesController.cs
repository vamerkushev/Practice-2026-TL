using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;

namespace WebApi2.Controllers;

[ApiController]
[Route( "api/roomtypes" )]
public class RoomTypesController : ControllerBase
{
    private readonly RoomTypeService _roomTypeService;
    public RoomTypesController( RoomTypeService roomTypeService )
    {
        _roomTypeService = roomTypeService;
    }

    [HttpGet( "/api/properties/{propertyId:guid}/roomtypes" )]
    public IActionResult GetRoomTypes( [FromRoute] Guid propertyId )
    {
        IReadOnlyList<RoomType> roomTypes = _roomTypeService.GetRoomTypesForProperty( propertyId );

        IEnumerable<RoomTypeDto> result = roomTypes.Select( RoomTypeDto.MapFromRoomType );

        return Ok( result );
    }

    [HttpPost( "/api/properties/{propertyId:guid}/roomtypes" )]
    public IActionResult CreateRoomType( [FromRoute] Guid propertyId, [FromBody] CreateRoomTypeDto request )
    {
        Guid roomTypeId = _roomTypeService.CreateRoomType( propertyId, request );
        return CreatedAtAction( nameof( GetRoomType ), new { id = roomTypeId }, roomTypeId );
    }

    [HttpGet( "{id:guid}" )]
    public IActionResult GetRoomType( [FromRoute] Guid id )
    {
        RoomType? roomType = _roomTypeService.GetById( id );
        if ( roomType == null )
        {
            return NotFound();
        }

        RoomTypeDto result = RoomTypeDto.MapFromRoomType( roomType );

        return Ok( result );
    }

    [HttpPut( "{id:guid}" )]
    public IActionResult UpdateRoomType( [FromRoute] Guid id, [FromBody] UpdateRoomTypeDto request )
    {
        _roomTypeService.UpdateRoomType( id, request );
        return NoContent();
    }

    [HttpDelete( "{id:guid}" )]
    public IActionResult DeleteRoomType( [FromRoute] Guid id )
    {
        _roomTypeService.DeleteRoomType( id );
        return NoContent();
    }
}
