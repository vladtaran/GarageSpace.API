using Microsoft.AspNetCore.Mvc;
using GarageSpaceAPI.Contracts;
using GarageSpaceAPI.Contracts.Common;
using GarageSpaceAPI.Contracts.Dto;
using GarageSpace.Services.Interfaces;

namespace GarageSpace.Controllers;

//[Authorize]
[ApiController]
[Route("api/garages")]
public class GaragesController : AuthorizedApiController
{
    private readonly IGarageService _service;

    public GaragesController(IGarageService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("search")]
    public async Task<IActionResult> Get([FromQuery] SearchOptions options)
    {
        var items = await _service.Search(options.Take, options.Skip);

        return Ok(new PageOf<GarageDto>
        {
            Items = items,
            Total = items.Count
        });
        
    }

    [HttpGet]
    [Route("getbyowner")]
    public async Task<IActionResult> GetByOwner([FromQuery] long ownerId)
    {
        try
        {
            var garage = await _service.GetGarageByOwner(ownerId);
            if (garage == null)
            {
                return NotFound();
            }

            return Ok(garage);
        }
        catch (Exception ex) 
        {
            // TODO Log exception
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request."); ;
        }
    }
}