using Microsoft.EntityFrameworkCore;
using GarageSpace.Models.Repository.EF;
using GarageSpace.Models.Repository.EF.CarJournal;
using GarageSpace.Models.Repository.EF.Vehicles;

namespace GarageSpace.Repository.EntityFramework
{
    public class MainDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<UserGarage> Garages { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<JournalRecord> JournalRecords { get; set; }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User-Garage relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Garage)
                .WithOne(g => g.Owner)
                .HasForeignKey<UserGarage>(g => g.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasOne(c => c.Garage)
                .WithMany(common => common.Vehicles)
                .HasForeignKey(c => c.GarageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Car>()
                .HasOne(b => b.Manufacturer);

            modelBuilder.Entity<Motorcycle>()
                .HasOne(b => b.Manufacturer);

            modelBuilder.Entity<Trailer>()
                .HasOne(b => b.Manufacturer);

            modelBuilder.Entity<Manufacturer>()
                .HasOne(c => c.ManufacturerCountry);

            modelBuilder.Entity<Manufacturer>()
                .Property(m => m.ManufacturerName)
                .HasConversion<string>();

            modelBuilder.Entity<Journal>()
                .HasOne(j => j.Vehicle)
                .WithOne(v => v.Journal)
                .HasForeignKey<Journal>(j => j.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Journal>()
                .HasOne(j => j.CreatedBy);

            modelBuilder.Entity<JournalRecord>()
                .HasOne(c => c.Journal);
        }
    }
}