using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarageSpace.Repository.EntityFramework;
using Xunit;
using User = GarageSpace.Models.Repository.EF.User;
using GenderEnum = GarageSpace.Data.Models.GenderEnum;

namespace GarageSpace.UnitTests.Repository;

public class UserRepositoryTests : IDisposable
{
    private readonly MainDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MainDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_ValidUser_ReturnsCreatedUser()
    {
        // Arrange
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com",
            Gender = GenderEnum.Male,
            DriverExperience = 5
        };

        // Act
        var result = await _repository.CreateAsync(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Nickname, result.Nickname);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Gender, result.Gender);
        Assert.Equal(user.DriverExperience, result.DriverExperience);
        Assert.NotEqual(0, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com",
            Gender = GenderEnum.Male,
            DriverExperience = 5
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Nickname, result.Nickname);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Gender, result.Gender);
        Assert.Equal(user.DriverExperience, result.DriverExperience);
    }

    [Fact]
    public async Task UpdateAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com",
            Gender = GenderEnum.Male,
            DriverExperience = 5
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        user.Name = "Updated User";
        var result = await _repository.UpdateAsync(user);

        // Assert
        Assert.True(result);
        var updatedUser = await _context.Users.FindAsync(user.Id);
        Assert.Equal("Updated User", updatedUser.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingUser_ReturnsTrue()
    {
        // Arrange
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com",
            Gender = GenderEnum.Male,
            DriverExperience = 5
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(user.Id);

        // Assert
        Assert.True(result);
        var deletedUser = await _context.Users.FindAsync(user.Id);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Name = "User 1", Nickname = "user1", Email = "user1@example.com", Gender = GenderEnum.Male, DriverExperience = 5 },
            new() { Name = "User 2", Nickname = "user2", Email = "user2@example.com", Gender = GenderEnum.Female, DriverExperience = 3 }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ListAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Name == "User 1");
        Assert.Contains(result, u => u.Name == "User 2");
    }

    [Fact]
    public async Task SearchAsync_WithPagination_ReturnsCorrectUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Name = "User 1", Nickname = "user1", Email = "user1@example.com", Gender = GenderEnum.Male, DriverExperience = 5 },
            new() { Name = "User 2", Nickname = "user2", Email = "user2@example.com", Gender = GenderEnum.Female, DriverExperience = 3 },
            new() { Name = "User 3", Nickname = "user3", Email = "user3@example.com", Gender = GenderEnum.Male, DriverExperience = 7 }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync(take: 2, skip: 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, u => u.Name == "User 2");
        Assert.Contains(result, u => u.Name == "User 3");
    }

    [Fact]
    public async Task GetByEmailAsync_ExistingUser_ReturnsUser()
    {
        // Arrange
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com",
            Gender = GenderEnum.Male,
            DriverExperience = 5
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByEmailAsync(user.Email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Nickname, result.Nickname);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Gender, result.Gender);
        Assert.Equal(user.DriverExperience, result.DriverExperience);
    }

    [Fact]
    public async Task GetByEmailAsync_NonExistingUser_ReturnsNull()
    {
        // Act
        var result = await _repository.GetByEmailAsync("nonexisting@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Name = "User 1", Nickname = "user1", Email = "user1@example.com", Gender = GenderEnum.Male, DriverExperience = 5 },
            new() { Name = "User 2", Nickname = "user2", Email = "user2@example.com", Gender = GenderEnum.Female, DriverExperience = 3 }
        };
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTotalCountAsync();

        // Assert
        Assert.Equal(2, result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 