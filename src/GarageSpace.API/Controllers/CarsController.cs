using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GarageSpace.API.Contracts;
using GarageSpace.API.Contracts.Request;
using GarageSpace.API.Contracts.Common;
using GarageSpace.Services.Interfaces;
using GarageSpace.API.Contracts.Dto.Vehicle;

namespace GarageSpace.Controllers;

//[Authorize]
[ApiController]
[Route("api/cars")]
public class CarsController : AuthorizedApiController
{
    private readonly ICarsService _carsService;

    public CarsController(ICarsService carsService)
    {
        _carsService = carsService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] SearchOptions options)
    {
        var items = await _carsService.Search(options.Take, options.Skip);

        return Ok(new PageOf<CarDto>
        {
            Items = items,
            Total = items.Count
        });
        
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CarDto car)
    {
        //if (CurrentUserId != null) car.CreatedBy = new Guid(CurrentUserId);

        var carToCreate = new CarDto
        {
            Name  = car.Name,
            Manufacturer = car.Manufacturer,
        };

        var id = await _carsService.Create(carToCreate);
        return CreatedAtAction(nameof(Get), new { id }, car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(long id, [FromBody] UpdateCarRequest request)
    {
        await _carsService.Update(id,
            new CarDto
            {
                //Model = request.Model,
                //Year = request.Year,
                //ImageId = request.ImageId
            });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _carsService.Delete(id);
        return NoContent();
    }
}