using GarageSpace.Data.Models.EF;
using GarageSpace.Repository.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace GarageSpace.IntegrationTests.Repository;

public class GaragesRepositoryIntegrationTests : BaseIntegrationTest<GaragesRepository>
{
    private readonly DatabaseFixture _fixture;

    public GaragesRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _fixture = fixture;
    }
    protected override GaragesRepository CreateRepository(MainDbContext dbContext)
    {
        return new GaragesRepository(dbContext);
    }

    [Fact]
    public async Task CreateAsync_ValidGarage_ReturnsCreatedGarage()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();

        User user = await CreateUserInDatabase(dbContext);

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };

        // Act
        await repository.CreateAsync(garage);

        // Assert
        var createdGarage = await dbContext.Garages.FindAsync(garage.Id);
        Assert.NotNull(createdGarage);
        Assert.Equal(user.Id, createdGarage.OwnerId);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingGarage_ReturnsGarage()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();
        User user = await CreateUserInDatabase(dbContext);

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        dbContext.Garages.Add(garage);
        await dbContext.SaveChangesAsync();


        // Act
        var result = await repository.GetByIdAsync(garage.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(garage.Id, result.Id);
        Assert.Equal(user.Id, result.OwnerId);
    }

    [Fact]
    public async Task UpdateAsync_ExistingGarage_ReturnsTrue()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();
        User user = await CreateUserInDatabase(dbContext);

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        dbContext.Garages.Add(garage);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.UpdateAsync(garage);

        // Assert
        Assert.True(result);
        var updatedGarage = await dbContext.Garages.FindAsync(garage.Id);
        Assert.NotNull(updatedGarage);
    }

    [Fact]
    public async Task DeleteAsync_ExistingGarage_ReturnsTrue()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();
        User user = await CreateUserInDatabase(dbContext);

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        dbContext.Garages.Add(garage);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.DeleteAsync(garage.Id);

        // Assert
        Assert.True(result);
        var deletedGarage = await dbContext.Garages.FindAsync(garage.Id);
        Assert.Null(deletedGarage);
    }

    [Fact]
    public async Task ListAllAsync_ReturnsAllGarages()
    {
        // Arrange
        await _fixture.CleanupDatabase();

        var (dbContext, repository) = CreateDbContext();

        var user1 = await CreateUserInDatabase(dbContext, "user1@email.com", "testName1", "testNickName1");
        var user2 = await CreateUserInDatabase(dbContext, "user2@email.com", "testName2", "testNickName2");

        var garages = new List<UserGarage>
        {
            new() { OwnerId = user1.Id },
            new() { OwnerId = user2.Id }
        };
        dbContext.Garages.AddRange(garages);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.ListAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, g => g.OwnerId == user1.Id);
        Assert.Contains(result, g => g.OwnerId == user2.Id);
    }

    [Fact]
    public async Task SearchAsync_WithPagination_ReturnsCorrectGarages()
    {
        // Arrange
        await _fixture.CleanupDatabase();

        var (dbContext, repository) = CreateDbContext();

        var user1 = await CreateUserInDatabase(dbContext, "user1@email.com", "testName1", "testNickName1");
        var user2 = await CreateUserInDatabase(dbContext, "user2@email.com", "testName2", "testNickName2");
        var user3 = await CreateUserInDatabase(dbContext, "user3@email.com", "testName3", "testNickName3");

        var garages = new List<UserGarage>
        {
            new() { OwnerId = user1.Id },
            new() { OwnerId = user2.Id },
            new() { OwnerId = user3.Id }
        };
        dbContext.Garages.AddRange(garages);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync(take: 2, skip: 1);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, g => g.OwnerId == user2.Id);
        Assert.Contains(result, g => g.OwnerId == user3.Id);
    }

    [Fact]
    public async Task GetTotalCountAsync_ReturnsCorrectCount()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();

        var user1 = await CreateUserInDatabase(dbContext, "user1@email.com", "testName1", "testNickName1");
        var user2 = await CreateUserInDatabase(dbContext, "user2@email.com", "testName2", "testNickName2");

        var garages = new List<UserGarage>
        {
            new() { OwnerId = user1.Id },
            new() { OwnerId = user2.Id }
        };

        dbContext.Garages.AddRange(garages);
        await dbContext.SaveChangesAsync();

        var expectedResult = await dbContext.Garages.CountAsync();

        // Act
        var actualResult = await repository.GetTotalCountAsync();

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task GetByOwnerIdAsync_ExistingGarage_ReturnsGarage()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();
        var user = await CreateUserInDatabase(dbContext);

        var garage = new UserGarage
        {
            OwnerId = user.Id
        };
        dbContext.Garages.Add(garage);
        await dbContext.SaveChangesAsync();

        // Act
        var result = await repository.GetByOwnerIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(garage.Id, result.Id);
        Assert.Equal(user.Id, result.OwnerId);
    }

    [Fact]
    public async Task GetByOwnerIdAsync_NonExistingGarage_ReturnsNull()
    {
        // Arrange
        var (dbContext, repository) = CreateDbContext();
        var user = await CreateUserInDatabase(dbContext);

        // Act
        var result = await repository.GetByOwnerIdAsync(user.Id);

        // Assert
        Assert.Null(result);
    }

    private static async Task<User> CreateUserInDatabase(MainDbContext dbContext, string? name = null, string? nickname = null, string? email = null)
    {
        var user = new User
        {
            Name = name ?? "testName",
            Nickname = nickname ?? "testNickName",
            Email = email ?? "test.email@email.com"
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }
} 