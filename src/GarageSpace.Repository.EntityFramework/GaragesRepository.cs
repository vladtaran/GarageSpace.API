using Microsoft.EntityFrameworkCore;
using GarageSpace.Repository.Interfaces.EF;
using UserGarage = GarageSpace.Models.Repository.EF.UserGarage;

namespace GarageSpace.Repository.EntityFramework;

public class GaragesRepository : BaseRepository<UserGarage>, IEFGaragesRepository
{
    private readonly MainDbContext _context;

    public GaragesRepository(MainDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IList<UserGarage>> SearchAsync(int take, int skip)
    {
        return await _context.Garages
            .Skip(skip)
            .Take(take)
            .Include(g => g.Cars)
                .ThenInclude(c => c.Manufacturer)
                .ThenInclude(m => m.ManufacturerCountry)
            .Include(g => g.Motorcycles)
                .ThenInclude(m => m.Manufacturer)
                .ThenInclude(m => m.ManufacturerCountry)
            .Include(g => g.Trailers)
                .ThenInclude(m => m.Manufacturer)
                .ThenInclude(m => m.ManufacturerCountry)
            .Include(g => g.Owner)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Garages.CountAsync();
    }

    public async Task<IList<UserGarage>> ListAllAsync()
    {
        return await _context.Garages
            .Include(g => g.Cars)
                .ThenInclude(c => c.Manufacturer)
            .Include(g => g.Motorcycles)
                .ThenInclude(m => m.Manufacturer)
            .Include(g => g.Trailers)
                .ThenInclude(m => m.Manufacturer)
            .Include(g => g.Owner)
            .ToListAsync();
    }

    public async Task<UserGarage?> GetByIdAsync(long id)
    {
        return await _context.Garages
            .Include(g => g.Cars)
                .ThenInclude(c => c.Manufacturer)
            .Include(g => g.Motorcycles)
                .ThenInclude(m => m.Manufacturer)
            .Include(g => g.Trailers)
                .ThenInclude(m => m.Manufacturer)
            .Include(g => g.Owner)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<UserGarage?> GetByOwnerIdAsync(long ownerId)
    {
        return await _context.Garages
            .Include(g => g.Cars)
                .ThenInclude(c => c.Manufacturer)
            .Include(g => g.Motorcycles)
                .ThenInclude(m => m.Manufacturer)
            .Include(g => g.Trailers)
                .ThenInclude(m => m.Manufacturer)
            .Include(g => g.Owner)
            .FirstOrDefaultAsync(x => x.OwnerId == ownerId);
    }

    public async Task<UserGarage> CreateAsync(UserGarage userGarage)
    {
        if (userGarage == null)
            throw new ArgumentNullException(nameof(userGarage));

        _context.Garages.Add(userGarage);
        await _context.SaveChangesAsync();
        return userGarage;
    }

    public async Task<bool> UpdateAsync(UserGarage userGarage)
    {
        if (userGarage == null)
            throw new ArgumentNullException(nameof(userGarage));

        var existingGarage = await _context.Garages.FindAsync(userGarage.Id);
        if (existingGarage == null)
            return false;

        _context.Entry(existingGarage).CurrentValues.SetValues(userGarage);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var garage = await _context.Garages.FindAsync(id);
        if (garage == null)
            return false;

        _context.Garages.Remove(garage);
        await _context.SaveChangesAsync();
        return true;
    }
}