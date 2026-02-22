using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GarageSpace.Repository.EntityFramework;
using Xunit;
using GarageSpace.Models.Repository.EF;
using GarageSpace.Models.Repository.EF.CarJournal;
using GarageSpace.Models.Repository.EF.Vehicles;

namespace GarageSpace.UnitTests.Repository;

public class JournalsRepositoryTests : IDisposable
{
    private readonly MainDbContext _context;
    private readonly JournalsRepository _repository;

    public JournalsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MainDbContext(options);
        _repository = new JournalsRepository(_context);
    }

    private async Task<(User user, Manufacturer manufacturer, UserGarage garage, List<Car> cars)> SetupTestData()
    {
        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var manufacturer = new Manufacturer
        {
            ManufacturerName = ManufacturerEnum.Toyota,
            YearCreation = 1937
        };
        _context.Manufacturers.Add(manufacturer);
        await _context.SaveChangesAsync();

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        _context.Garages.Add(garage);
        await _context.SaveChangesAsync();

        var cars = new List<Car>
        {
            new() { Name = "Test Car 1", ManufacturerId = manufacturer.Id, GarageId = garage.Id, LicensePlate = "ABC123" },
            new() { Name = "Test Car 2", ManufacturerId = manufacturer.Id, GarageId = garage.Id, LicensePlate = "DEF456" },
            new() { Name = "Test Car 3", ManufacturerId = manufacturer.Id, GarageId = garage.Id, LicensePlate = "GHI789" }
        };
        _context.Cars.AddRange(cars);
        await _context.SaveChangesAsync();

        return (user, manufacturer, garage, cars);
    }

    [Fact]
    public async Task AddAsync_ValidJournal_AddsJournalToDatabase()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();
        var car = cars[0];

        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };

        // Act
        await _repository.AddAsync(journal);
        await _context.SaveChangesAsync();

        // Assert
        var addedJournal = await _context.Journals.FindAsync(journal.Id);
        Assert.NotNull(addedJournal);
        Assert.Equal(journal.Title, addedJournal.Title);
        Assert.Equal(car.Id, addedJournal.VehicleId);
        Assert.Equal(user.Id, addedJournal.CreatedById);
        Assert.NotEqual(0, addedJournal.Id);
    }

    [Fact]
    public async Task GetByVehicleId_ExistingJournal_ReturnsJournal()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();
        var car = cars[0];

        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };
        _context.Journals.Add(journal);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByVehicleId(car.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(journal.Title, result.First().Title);
        Assert.Equal(car.Id, result.First().VehicleId);
        Assert.Equal(user.Id, result.First().CreatedById);
    }

    [Fact]
    public async Task UpdateAsync_ExistingJournal_UpdatesJournal()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();
        var car = cars[0];

        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };
        _context.Journals.Add(journal);
        await _context.SaveChangesAsync();

        // Act
        journal.Title = "Updated Journal";
        await _repository.UpdateAsync(journal);
        await _context.SaveChangesAsync();

        // Assert
        var updatedJournal = await _context.Journals.FindAsync(journal.Id);
        Assert.Equal("Updated Journal", updatedJournal.Title);
    }

    [Fact]
    public async Task DeleteAsync_ExistingJournal_RemovesJournal()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();
        var car = cars[0];

        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };
        _context.Journals.Add(journal);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(journal.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedJournal = await _context.Journals.FindAsync(journal.Id);
        Assert.Null(deletedJournal);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllJournals()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();

        var journals = new List<Journal>
        {
            new() { VehicleId = cars[0].Id, CreatedById = user.Id, Title = "Journal 1" },
            new() { VehicleId = cars[1].Id, CreatedById = user.Id, Title = "Journal 2" }
        };
        _context.Journals.AddRange(journals);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ListAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, j => j.Title == "Journal 1");
        Assert.Contains(result, j => j.Title == "Journal 2");
    }

    [Fact]
    public async Task SearchAsync_WithPagination_ReturnsCorrectJournals()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();

        var journals = new List<Journal>
        {
            new() { VehicleId = cars[0].Id, CreatedById = user.Id, Title = "Journal 1" },
            new() { VehicleId = cars[1].Id, CreatedById = user.Id, Title = "Journal 2" },
            new() { VehicleId = cars[2].Id, CreatedById = user.Id, Title = "Journal 3" }
        };
        _context.Journals.AddRange(journals);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync(take: 2, skip: 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, j => j.Title == "Journal 2");
        Assert.Contains(result, j => j.Title == "Journal 3");
    }

    [Fact]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var (user, manufacturer, garage, cars) = await SetupTestData();

        var journals = new List<Journal>
        {
            new() { VehicleId = cars[0].Id, CreatedById = user.Id, Title = "Journal 1" },
            new() { VehicleId = cars[1].Id, CreatedById = user.Id, Title = "Journal 2" }
        };
        _context.Journals.AddRange(journals);
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