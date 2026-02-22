using GarageSpace.Repository.EntityFramework;
using GarageSpace.Repository.Interfaces.EF;

namespace GarageSpace.IntegrationTests.Repository;

public abstract class BaseIntegrationTest<TRepository>(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    where TRepository : IBaseRepository
{
    private readonly DatabaseFixture _fixture = fixture;

    protected abstract TRepository CreateRepository(MainDbContext dbContext);

    protected (MainDbContext, TRepository) CreateDbContext()
    {
        var dbContext = _fixture.CreateDbContext();
        var repository = CreateRepository(dbContext);
        return (dbContext, repository);
    }
} 