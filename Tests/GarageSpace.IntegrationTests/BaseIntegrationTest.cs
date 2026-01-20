using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using GarageSpace.Repository.EntityFramework;

namespace GarageSpace.IntegrationTests;

public abstract class BaseIntegrationTest : IDisposable
{
    protected readonly MainDbContext DbContext;
    protected readonly IConfiguration Configuration;

    protected BaseIntegrationTest()
    {
        // Load configuration from appsettings.json
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Setup SQL Server database for testing
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            .Options;

        DbContext = new MainDbContext(options);
        
        // Ensure database schema exists (apply EF Core migrations)
        DbContext.Database.Migrate();
    }

    protected async Task CleanupDatabase()
    {
        // Remove all data from all tables
        DbContext.Countries.RemoveRange(DbContext.Countries);
        DbContext.Journals.RemoveRange(DbContext.Journals);
        DbContext.JournalRecords.RemoveRange(DbContext.JournalRecords);
        DbContext.Cars.RemoveRange(DbContext.Cars);
        DbContext.Garages.RemoveRange(DbContext.Garages);
        DbContext.Users.RemoveRange(DbContext.Users);
        DbContext.Manufacturers.RemoveRange(DbContext.Manufacturers);
        
        await DbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        //DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
} 