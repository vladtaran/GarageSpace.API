using Microsoft.EntityFrameworkCore;
using GarageSpace.Repository.Interfaces.EF;
using Car = GarageSpace.Models.Repository.EF.Vehicles.Car;

namespace GarageSpace.Repository.EntityFramework;

public class CarsRepository : BaseRepository<Car>, IEFCarsRepository
{
    private readonly MainDbContext _context;

    public CarsRepository(MainDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Car> CreateAsync(Car car)
    {
        if (car == null)
            throw new ArgumentNullException(nameof(car));

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
        return car;
    }

    public async Task<IList<Car>> ListAllAsync()
    {
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .ToListAsync();
    }

    public async Task<Car?> GetByIdAsync(long id)
    {
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> UpdateAsync(Car updatedCar)
    {
        if (updatedCar == null)
            throw new ArgumentNullException(nameof(updatedCar));

        var existingCar = await _context.Cars.FindAsync(updatedCar.Id);
        if (existingCar == null)
            return false;

        _context.Entry(existingCar).CurrentValues.SetValues(updatedCar);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
            return false;

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IList<Car>> SearchAsync(int take, int skip)
    {
        return await _context.Cars
            .Include(c => c.Manufacturer)
            .OrderBy(c => c.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Cars.CountAsync();
    }
}