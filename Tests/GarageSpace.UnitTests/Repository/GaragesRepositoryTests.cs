using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarageSpace.Repository.EntityFramework;
using Xunit;
using GarageSpace.Models.Repository.EF;

namespace GarageSpace.UnitTests.Repository;

public class GaragesRepositoryTests : IDisposable
{
    private readonly MainDbContext _context;
    private readonly GaragesRepository _repository;

    public GaragesRepositoryTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MainDbContext(options);
        _repository = new GaragesRepository(_context);
    }

    private async Task<User> SetupTestData()
    {
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task CreateAsync_ValidGarage_ReturnsCreatedGarage()
    {
        // Arrange
        var user = await SetupTestData();

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };

        // Act
        var result = await _repository.CreateAsync(garage);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.OwnerId);
        Assert.NotEqual(0, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ExistingGarage_ReturnsTrue()
    {
        // Arrange
        var user = await SetupTestData();

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        _context.Garages.Add(garage);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.UpdateAsync(garage);
        await _context.SaveChangesAsync();

        // Assert
        Assert.True(result);
        var updatedGarage = await _context.Garages.FindAsync(garage.Id);
        Assert.NotNull(updatedGarage);
        Assert.Equal(user.Id, updatedGarage.OwnerId);
    }

    [Fact]
    public async Task DeleteAsync_ExistingGarage_ReturnsTrue()
    {
        // Arrange
        var user = await SetupTestData();

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        _context.Garages.Add(garage);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(garage.Id);
        await _context.SaveChangesAsync();

        // Assert
        Assert.True(result);
        var deletedGarage = await _context.Garages.FindAsync(garage.Id);
        Assert.Null(deletedGarage);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllGarages()
    {
        // Arrange
        var user1 = await SetupTestData();
        var user2 = await SetupTestData();

        var garages = new List<UserGarage>
        {
            new() { OwnerId = user1.Id },
            new() { OwnerId = user2.Id }
        };
        _context.Garages.AddRange(garages);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ListAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task SearchAsync_WithPagination_ReturnsCorrectGarages()
    {
        // Arrange
        var user1 = await SetupTestData();
        var user2 = await SetupTestData();
        var user3 = await SetupTestData();

        var garages = new List<UserGarage>
        {
            new() { OwnerId = user1.Id },
            new() { OwnerId = user2.Id },
            new() { OwnerId = user3.Id }
        };
        _context.Garages.AddRange(garages);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync(take: 2, skip: 1);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var user1 = await SetupTestData();
        var user2 = await SetupTestData();

        var garages = new List<UserGarage>
        {
            new() { OwnerId = user1.Id },
            new() { OwnerId = user2.Id }
        };
        _context.Garages.AddRange(garages);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetTotalCountAsync();

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetByOwnerIdAsync_ExistingGarage_ReturnsGarage()
    {
        // Arrange
        var user = await SetupTestData();

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        _context.Garages.Add(garage);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByOwnerIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.OwnerId);
    }

    [Fact]
    public async Task GetByOwnerIdAsync_NonExistingGarage_ReturnsNull()
    {
        // Arrange
        var user = await SetupTestData();

        // Act
        var result = await _repository.GetByOwnerIdAsync(user.Id);

        // Assert
        Assert.Null(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
} 