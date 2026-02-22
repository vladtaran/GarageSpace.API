using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Threading.Tasks;
using GarageSpace.API.Contracts;
using GarageSpace.API.Contracts.Dto;
using GarageSpace.Repository.Interfaces.EF;
using GarageSpace.Services;
using GarageSpace.Services.Interfaces;
using Xunit;
using GarageSpace.EventBus.SDK.Abstractions;
using GarageSpace.Models.Repository.EF;

namespace GarageSpace.UnitTests.Service;

public class UsersServiceTest
{
    private IEFUserRepository _userRepositoryMock;
    private IUsersService _usersService;
    private IPasswordHasher<Services.Models.User> _passwordHasherMock;
    private IOptions<AppSettings> _appSettings;
    private ILogger<UsersService> _usersServiceLoggerMock;
    private IMapper _mapperMock;
    private IEventBusPublisher _eventsPublisher;

    public UsersServiceTest()
    {
        _userRepositoryMock = Substitute.For<IEFUserRepository>();
        _appSettings = Substitute.For<IOptions<AppSettings>>();
        _passwordHasherMock = Substitute.For<IPasswordHasher<Services.Models.User>>();
        _usersServiceLoggerMock = Substitute.For<ILogger<UsersService>>();
        _mapperMock = Substitute.For<IMapper>();
        _eventsPublisher = Substitute.For<IEventBusPublisher>();

        _usersService = new UsersService(_userRepositoryMock, _appSettings, _passwordHasherMock, _usersServiceLoggerMock, _mapperMock, _eventsPublisher);
    }

    [Fact]
    public async Task TryToRegisterUser_Success()
    {
        //Given 
        
        var userRequest = new UserDto
        {
            Email = "mygarage@g.c",
            Nickname = "My Garage Username",
            Password = "mygaragepassword1!"
        };

        var expectedId = 1L;
        var mockUser = new User { Id = expectedId, Email = "test@g.com", Name = "testName", Nickname = "TestNickName" };
        _userRepositoryMock.CreateAsync(Arg.Any<User>()).Returns(mockUser);

        _passwordHasherMock.HashPassword(Arg.Any<Services.Models.User>(), Arg.Any<string>()).Returns(string.Empty);
        
        // When
        
        var id = await _usersService.Register(userRequest);
        
        //Then
        
        Assert.Equal(expectedId, id);
        
    }
    
    [Fact]
    public async Task TryToRegisterUser_Fail_UserExists()
    {
        //Given 
        var userRequest = new UserDto
        {
            Email = "mygarage@g.c",
            Nickname = "My Garage Username",
            Password = "mygaragepassword1!"
        };

        var expectedId = 1L;

        // When

        var mockUser = new User { Email = "test@g.com", Name = "testName", Nickname = "TestNickName" };
        _userRepositoryMock.GetByEmailAsync(Arg.Any<string>()).Returns<User>(mockUser);
        
        //Then
        
        var ex = await Assert.ThrowsAsync<ApplicationException>(() => _usersService.Register(userRequest));
        Assert.NotNull(ex);
        Assert.Equal("User already exists", ex?.Message);
        
    }
}