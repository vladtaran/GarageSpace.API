using GarageSpace.Models.Repository.EF.Vehicles;
using GarageSpace.Repository.EntityFramework;
using GarageSpace.Models.Repository.EF.CarJournal;
using GarageSpace.Data.Models;
using GarageSpace.Models.Repository.EF;

namespace GarageSpace
{
    public class MockData
    {
        public static void SetupMockData(MainDbContext context)
        {
            if (context.Garages.Any())
                return; // Already seeded

            // Create Countries
            var country1 = new Country { Name = "United States of America", CountryCode = "USA" };
            var country2 = new Country { Name = "Japan", CountryCode = "JP" };


            // Create Manufacturer
            var manufacturerDodge = new Manufacturer { ManufacturerCountry = country1, ManufacturerName = ManufacturerEnum.Dodge, YearCreation = 1986 };
            var manufacturerToyota = new Manufacturer { ManufacturerCountry = country2, ManufacturerName = ManufacturerEnum.Toyota, YearCreation = 1901 };
            var manufacturerSuzuki = new Manufacturer { ManufacturerCountry = country2, ManufacturerName = ManufacturerEnum.Suzuki, YearCreation = 1964 };
            var manufacturerKawasaki = new Manufacturer { ManufacturerCountry = country2, ManufacturerName = ManufacturerEnum.Kawasaki, YearCreation = 1950 };
            var manufacturerLev = new Manufacturer { ManufacturerCountry = country2, ManufacturerName = ManufacturerEnum.Kawasaki, YearCreation = 1950 };

            //Create Users
            var user1 = new User { Name = "Vladyslav", Surname = "Tar", Nickname = "Vynd", DriverExperience = 10, Email = "vlad@gmail.com", Phone = "1234567890", Gender = GenderEnum.Male, CreatedAt = DateTime.UtcNow };
            var user2 = new User { Name = "Serg", Surname = "Postkevich", Nickname = "Geek", DriverExperience = 6, Email = "geek@gmail.com", Phone = "123654787", Gender = GenderEnum.Male, CreatedAt = DateTime.UtcNow };


            // Create a Common entity
            var garage1 = new UserGarage { Owner = user1, CreatedAt = DateTime.UtcNow };
            var garage2 = new UserGarage { Owner = user2, CreatedAt = DateTime.UtcNow };

            // Create some Bs
            var b1 = new Car
            {
                Name = "Toyota Tundra",
                Year = "2017",
                Body = "Sedan",
                Manufacturer = manufacturerToyota,
                Garage = garage1,
                LicensePlate = "AI6478KK",
                CreatedAt = DateTime.UtcNow,
                Engine = "5000cc",
                HorsePower = 250,
                NumberOfDoors = 5,
                Transmission = TransmitionTypes.Automatic,
                FuelType = FuelTypes.Disel,
                Trim = "Pickup",
                Weight = 3500,
                VIN = "EU2222222222222222"
            };
            var b2 = new Car
            {
                Garage = garage2,
                Name = "Suzuki Jimny",
                Year = "2008",
                Body = "Hatchback",
                Manufacturer = manufacturerSuzuki,
                LicensePlate = "AE2654YB",
                CreatedAt = DateTime.UtcNow,
                Engine = "1600cc",
                HorsePower = 130,
                NumberOfDoors = 5,
                Transmission = TransmitionTypes.Manual,
                FuelType = FuelTypes.Gasoline,
                Trim = "Crossover",
                Weight = 2050,
                VIN = "EU123192378123712371"
            };
            var b3 = new Car
            {
                Garage = garage1,
                Name = "Dodge Journey",
                Year = "2014",
                Body = "Crossover",
                Manufacturer = manufacturerDodge,
                LicensePlate = "KY1564YA",
                CreatedAt = DateTime.UtcNow,
                Engine = "2500cc",
                HorsePower = 180,
                NumberOfDoors = 4,
                Transmission = TransmitionTypes.CVT,
                FuelType = FuelTypes.LPGAndGasoline,
                Trim = "Crossover",
                Weight = 2050,
                VIN = "EU123192378123712371"
            };

            // Create motos
            var c1 = new Motorcycle { Garage = garage1, Name = "Kawasaki", Year = "2020", Manufacturer = manufacturerKawasaki, LicensePlate = "AE123", Type = "Sport", CreatedAt = DateTime.UtcNow, HorsePower = 50, HasSideCar = true };
            var c2 = new Motorcycle { Garage = garage2, Name = "Suzuki Bandit 1.8", Year = "2005", Manufacturer = manufacturerSuzuki, LicensePlate = "AE133", Type = "Cross", CreatedAt = DateTime.UtcNow };

            // Create trailers
            var t1 = new Trailer { Garage = garage1, Name = "Custom Trailer", Year = "2006", Manufacturer = manufacturerLev, LicensePlate = "AE8975XM", CreatedAt = DateTime.UtcNow, HasBrakes = true, LengthMeters = 32, LoadCapacityKg = 1000, NumberOfAxles = 2, TrailerType = "Flatbad" };

            // Create Journals
            var j1 = new Journal { CreatedBy = user1, Title = "Dodge Journey Journal", Vehicle = b3 };

            // Create journal Records
            var jr1 = new JournalRecord { EntryDate =  DateTime.Today, Description = "I bought car after couple of months searching and very proud to find such a good car.", Journal = j1, Title = "Car Buying" };
            var jr2 = new JournalRecord { EntryDate =  DateTime.Today, Description = "I have added a new exchaus to my car and it is very nice!", Journal = j1, Title = "First tuning" };

            // Add everything via Common
            context.Countries.Add(country1);
            context.Countries.Add(country2);

            context.Manufacturers.Add(manufacturerDodge);
            context.Manufacturers.Add(manufacturerToyota);
            context.Manufacturers.Add(manufacturerSuzuki);
            context.Manufacturers.Add(manufacturerKawasaki);
            context.Manufacturers.Add(manufacturerLev);

            context.Users.Add(user1);
            context.Users.Add(user2);

            context.Cars.Add(b1);
            context.Cars.Add(b2);
            context.Cars.Add(b3);

            context.Motorcycles.Add(c1);
            context.Motorcycles.Add(c2);

            context.Trailers.Add(t1);

            context.Garages.Add(garage1);
            context.Garages.Add(garage2);
            
            context.Journals.Add(j1);
            
            context.JournalRecords.Add(jr1);
            context.JournalRecords.Add(jr2);

            context.SaveChanges();
        }
    }
}
