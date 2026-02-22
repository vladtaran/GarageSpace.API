using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GarageSpace.Repository.EntityFramework;
using Xunit;
using GenderEnum = GarageSpace.Data.Models.GenderEnum;
using GarageSpace.Models.Repository.EF;
using GarageSpace.Models.Repository.EF.Vehicles;

namespace GarageSpace.UnitTests.Repository;

public class CarsRepositoryTests : IDisposable
{
    private readonly MainDbContext _context;
    private readonly CarsRepository _repository;

    public CarsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MainDbContext(options);
        _repository = new CarsRepository(_context);
    }

    private async Task<(Manufacturer manufacturer, UserGarage garage)> SetupTestData()
    {
        var manufacturer = new Manufacturer
        {
            ManufacturerName = ManufacturerEnum.Toyota,
            YearCreation = 1937,
            ManufacturerCountryId = 1
        };
        _context.Manufacturers.Add(manufacturer);

        var user = new User
        {
            Name = "Test User",
            Nickname = "testuser",
            Email = "test@example.com",
            Gender = GenderEnum.Male,
            DriverExperience = 5
        };
        _context.Users.Add(user);

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        _context.Garages.Add(garage);

        await _context.SaveChangesAsync();
        return (manufacturer, garage);
    }

    [Fact]
    public async Task CreateAsync_ValidCar_ReturnsCreatedCar()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var car = new Car
        {
            Name = "Test Car",
            ManufacturerId = manufacturer.Id,
            GarageId = garage.Id,
            LicensePlate = "TEST123",
            Transmission = TransmitionTypes.Automatic,
            NumberOfDoors = 4,
            FuelType = FuelTypes.Gasoline
        };

        // Act
        var result = await _repository.CreateAsync(car);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(car.Name, result.Name);
        Assert.Equal(manufacturer.Id, result.ManufacturerId);
        Assert.Equal(garage.Id, result.GarageId);
        Assert.Equal("TEST123", result.LicensePlate);
        Assert.NotEqual(0, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingCar_ReturnsCar()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var car = new Car
        {
            Name = "Test Car",
            ManufacturerId = manufacturer.Id,
            GarageId = garage.Id,
            LicensePlate = "TEST123",
            Transmission = TransmitionTypes.Automatic,
            NumberOfDoors = 4,
            FuelType = FuelTypes.Gasoline
        };
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(car.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(car.Name, result.Name);
        Assert.Equal(manufacturer.Id, result.ManufacturerId);
        Assert.Equal(garage.Id, result.GarageId);
        Assert.Equal("TEST123", result.LicensePlate);
    }

    [Fact]
    public async Task UpdateAsync_ExistingCar_ReturnsTrue()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var car = new Car
        {
            Name = "Test Car",
            ManufacturerId = manufacturer.Id,
            GarageId = garage.Id,
            LicensePlate = "TEST123",
            Transmission = TransmitionTypes.Automatic,
            NumberOfDoors = 4,
            FuelType = FuelTypes.Gasoline
        };
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        // Act
        car.Name = "Updated Car";
        var result = await _repository.UpdateAsync(car);

        // Assert
        Assert.True(result);
        var updatedCar = await _context.Cars.FindAsync(car.Id);
        Assert.Equal("Updated Car", updatedCar.Name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingCar_ReturnsTrue()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var car = new Car
        {
            Name = "Test Car",
            ManufacturerId = manufacturer.Id,
            GarageId = garage.Id,
            LicensePlate = "TEST123",
            Transmission = TransmitionTypes.Automatic,
            NumberOfDoors = 4,
            FuelType = FuelTypes.Gasoline
        };
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(car.Id);

        // Assert
        Assert.True(result);
        var deletedCar = await _context.Cars.FindAsync(car.Id);
        Assert.Null(deletedCar);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllCars()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var cars = new List<Car>
        {
            new() 
            { 
                Name = "Car 1", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR1",
                Transmission = TransmitionTypes.Automatic,
                NumberOfDoors = 4,
                FuelType = FuelTypes.Gasoline
            },
            new() 
            { 
                Name = "Car 2", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR2",
                Transmission = TransmitionTypes.Manual,
                NumberOfDoors = 2,
                FuelType = FuelTypes.Disel
            }
        };
        _context.Cars.AddRange(cars);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.ListAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Car 1");
        Assert.Contains(result, c => c.Name == "Car 2");
    }

    [Fact]
    public async Task SearchAsync_WithPagination_ReturnsCorrectCars()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var cars = new List<Car>
        {
            new() 
            { 
                Name = "Car 1", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR1",
                Transmission = TransmitionTypes.Automatic,
                NumberOfDoors = 4,
                FuelType = FuelTypes.Gasoline
            },
            new() 
            { 
                Name = "Car 2", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR2",
                Transmission = TransmitionTypes.Manual,
                NumberOfDoors = 2,
                FuelType = FuelTypes.Disel
            },
            new() 
            { 
                Name = "Car 3", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR3",
                Transmission = TransmitionTypes.CVT,
                NumberOfDoors = 5,
                FuelType = FuelTypes.Electro
            }
        };
        _context.Cars.AddRange(cars);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync(take: 2, skip: 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Car 2");
        Assert.Contains(result, c => c.Name == "Car 3");
    }

    [Fact]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var (manufacturer, garage) = await SetupTestData();

        var cars = new List<Car>
        {
            new() 
            { 
                Name = "Car 1", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR1",
                Transmission = TransmitionTypes.Automatic,
                NumberOfDoors = 4,
                FuelType = FuelTypes.Gasoline
            },
            new() 
            { 
                Name = "Car 2", 
                ManufacturerId = manufacturer.Id,
                GarageId = garage.Id,
                LicensePlate = "CAR2",
                Transmission = TransmitionTypes.Manual,
                NumberOfDoors = 2,
                FuelType = FuelTypes.Disel
            }
        };
        _context.Cars.AddRange(cars);
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