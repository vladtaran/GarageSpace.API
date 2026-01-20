using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GarageSpace.Repository.EntityFramework;

namespace GarageSpace.IntegrationTests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            await EnsureDatabaseMigrated();
            await CleanupDatabase();
        }

        public async Task DisposeAsync()
        {
            await CleanupDatabase();
        }

        private static MainDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var options = new DbContextOptionsBuilder<MainDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .Options;

            return new MainDbContext(options);
        }

        private static async Task EnsureDatabaseMigrated()
        {
            await using var dbContext = CreateDbContext();
            await dbContext.Database.MigrateAsync();
        }

        public async Task CleanupDatabase()
        {
            await EnsureDatabaseMigrated();
            await using var dbContext = CreateDbContext();

            dbContext.Countries.RemoveRange(dbContext.Countries);
            dbContext.Journals.RemoveRange(dbContext.Journals);
            dbContext.JournalRecords.RemoveRange(dbContext.JournalRecords);
            dbContext.Cars.RemoveRange(dbContext.Cars);
            dbContext.Garages.RemoveRange(dbContext.Garages);
            dbContext.Users.RemoveRange(dbContext.Users);
            dbContext.Manufacturers.RemoveRange(dbContext.Manufacturers);

            await dbContext.SaveChangesAsync();
        }
    }
}
