using GarageSpace.Data.Models.EF;
using GarageSpace.Data.Models.EF.CarJournal;
using GarageSpace.Data.Models.EF.Vehicles;
using GarageSpace.Repository.EntityFramework;

namespace GarageSpace.IntegrationTests.Repository;

public class JournalsRepositoryIntegrationTests : BaseIntegrationTest<JournalsRepository>
{
    private readonly DatabaseFixture _fixture;

    public JournalsRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
    }
    protected override JournalsRepository CreateRepository(MainDbContext dbContext)
    {
        return new JournalsRepository(dbContext);
    }

    [Fact]
    public async Task AddAsync_ValidJournal_AddsJournalToDatabase()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();

        var (user, manufacturer, garage, car) = await SetupTestData(dbContext);

        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };

        // Act
        await repository.AddAsync(journal);

        // Assert
        var addedJournal = await dbContext.Journals.FindAsync(journal.Id);
        Assert.NotNull(addedJournal);
        Assert.Equal(journal.Title, addedJournal.Title);
        Assert.Equal(car.Id, addedJournal.VehicleId);
        Assert.Equal(user.Id, addedJournal.CreatedById);
    }

    [Fact]
    public async Task GetByVehicleId_ExistingJournal_ReturnsJournal()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();

        var (user, manufacturer, garage, car) = await SetupTestData(dbContext);
        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };
        dbContext.Journals.Add(journal);
        await dbContext.SaveChangesAsync();

        var result = await repository.GetByVehicleId(car.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(journal.Title, result.First().Title);
        Assert.Equal(car.Id, result.First().VehicleId);
    }


    [Fact]
    public async Task DeleteAsync_ExistingJournal_RemovesJournal()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();

        var (user, manufacturer, garage, car) = await SetupTestData(dbContext);
        var journal = new Journal
        {
            VehicleId = car.Id,
            CreatedById = user.Id,
            Title = "Test Journal"
        };
        dbContext.Journals.Add(journal);
        await dbContext.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(journal.Id);

        // Assert
        var deletedJournal = await dbContext.Journals.FindAsync(journal.Id);
        Assert.Null(deletedJournal);
    }

    [Fact]
    public async Task SearchAsync_WithPagination_ReturnsCorrectJournals()
    {
        // Arrange
        // Verify database is clean
        await _fixture.CleanupDatabase();

        var (dbContext, repository) = CreateDbContext();

        var (user1, user2, manufacturer, garage1, garage2, car1, car2) = await SetupMultipleTestData(dbContext);
        var journals = new List<Journal>
        {
            new() { VehicleId = car1.Id, CreatedById = user1.Id, Title = "Journal Car 1" },
            new() { VehicleId = car2.Id, CreatedById = user2.Id, Title = "Journal Car 2" },
        };
        dbContext.Journals.AddRange(journals);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync(take: 2, skip: 0);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, j => j.Title == "Journal Car 1");
        Assert.Contains(result, j => j.Title == "Journal Car 2");
    }

    [Fact]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        // Verify database is clean
        await _fixture.CleanupDatabase();

        var (dbContext, repository) = CreateDbContext();

        var (user1, user2, manufacturer, garage1, garage2, car1, car2) = await SetupMultipleTestData(dbContext);
        var journals = new List<Journal>
        {
            new() { VehicleId = car1.Id, CreatedById = user1.Id, Title = "Journal 1" },
            new() { VehicleId = car2.Id, CreatedById = user2.Id, Title = "Journal 2" }
        };
        dbContext.Journals.AddRange(journals);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetTotalCountAsync();

        // Assert
        Assert.Equal(2, result);
    }

    private async Task<(User user, Manufacturer manufacturer, UserGarage garage, Car car)> SetupTestData(MainDbContext dbContext)
    {
        var user = new User
        {
            Name = "testName",
            Nickname = "testNickName",
            Email = "test.email@email.com"
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var country = new Country { Name = "Ukraine", CountryCode = "UA" };
        dbContext.Countries.Add(country);
        await dbContext.SaveChangesAsync();

        var manufacturer = new Manufacturer
        {
            ManufacturerName = ManufacturerEnum.Toyota,
            YearCreation = 1937,
            ManufacturerCountryId = country.Id
        };
        dbContext.Manufacturers.Add(manufacturer);
        await dbContext.SaveChangesAsync();

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        dbContext.Garages.Add(garage);
        await dbContext.SaveChangesAsync();

        var car = new Car
        {
            Name = "Test Car",
            ManufacturerId = manufacturer.Id,
            GarageId = garage.Id,
            LicensePlate = "ABC123"
        };
        dbContext.Cars.Add(car);
        await dbContext.SaveChangesAsync();

        return (user, manufacturer, garage, car);
    }

    private async Task<(User user1, User user2, Manufacturer manufacturer, UserGarage garage1, UserGarage garage2, Car car1, Car car2)> SetupMultipleTestData(MainDbContext dbContext)
    {
        var user1 = new User
        {
            Name = "testName1",
            Nickname = "testNickName1",
            Email = "test1.email@email.com"
        };
        var user2 = new User
        {
            Name = "testName2",
            Nickname = "testNickName2",
            Email = "test2.email@email.com"
        };
        dbContext.Users.AddRange(user1, user2);
        await dbContext.SaveChangesAsync();

        var country = new Country { Name = "United States", CountryCode = "USA" };
        dbContext.Countries.Add(country);
        await dbContext.SaveChangesAsync();

        var manufacturer = new Manufacturer
        {
            ManufacturerName = ManufacturerEnum.Toyota,
            YearCreation = 1937,
            ManufacturerCountryId = country.Id
        };
        dbContext.Manufacturers.Add(manufacturer);
        await dbContext.SaveChangesAsync();

        var garage1 = new UserGarage
        {
            OwnerId = user1.Id
        };
        var garage2 = new UserGarage
        {
            OwnerId = user2.Id
        };
        dbContext.Garages.AddRange(garage1, garage2);
        await dbContext.SaveChangesAsync();

        var car1 = new Car
        {
            Name = "Test Car 1",
            ManufacturerId = manufacturer.Id,
            GarageId = garage1.Id,
            LicensePlate = "ABC123"
        };
        var car2 = new Car
        {
            Name = "Test Car 2",
            ManufacturerId = manufacturer.Id,
            GarageId = garage2.Id,
            LicensePlate = "DEF456"
        };
        dbContext.Cars.AddRange(car1, car2);
        await dbContext.SaveChangesAsync();

        return (user1, user2, manufacturer, garage1, garage2, car1, car2);
    }

} 