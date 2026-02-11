using AutoMapper;
using GarageSpace.API.Mapping;
using GarageSpace.Controllers.Request;
using GarageSpace.Services.Interfaces;
using GarageSpaceAPI.Contracts.Dto;
using GarageSpaceAPI.Contracts.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GarageSpace.Controllers;

[Authorize]
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly ICarsService _carsService;
    private readonly IMapper _mapper;
    private readonly IUserObjectsMapper _userObjectsMapper;
    
    public UserController(
        IUsersService usersService,
        ICarsService carsService,
        IMapper mapper,
        IUserObjectsMapper userObjectsMapper)
    {
        _usersService = usersService;
        _carsService = carsService;
        _mapper = mapper;
        _userObjectsMapper = userObjectsMapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var user = await _usersService.GetUserById(id);
        var cars = await _carsService.GetByUserId(id);

        return Ok(new GarageDto
        {
            Cars = cars
        });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetByName([FromQuery] string name)
    {
        var user = await _usersService.GetByNickname(name);
        if (user == null)
        {
            return NotFound();
        }

        var cars = await _carsService.GetByUserId(user.Id);

        return Ok(new GarageDto
        {
            Cars = cars
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, UpdateUserRequest request)
    {
        var user = _mapper.Map<UserDto>(request);
       var isUpdated = await _usersService.UpdateUser(id, user);

       if (isUpdated)
       {
           return Ok(isUpdated);
       }

       return NotFound();
    }
    
    [AllowAnonymous]
    [HttpGet("exists")]
    public async Task<IActionResult> CheckUserExists([FromQuery] CheckUserExistRequest request)
    {
        var isUserExists = await _usersService.CheckUserExists(request.Property, request.Value);
        return Ok(isUserExists);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(UserCreateRequest request)
    {
        var user = _userObjectsMapper.MapCreateUserRequestToUser(request);

        long? userId = await _usersService.RegisterUser(user);

        if (userId.HasValue)
        {
            return Ok(userId.Value);
        }

        return BadRequest("Failed to register a new user.");
    }
}