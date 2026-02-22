using Microsoft.EntityFrameworkCore;
using GarageSpace.Repository.Interfaces.EF;
using GarageSpace.Models.Repository.EF.CarJournal;

namespace GarageSpace.Repository.EntityFramework;

public class JournalsRepository : BaseRepository<Journal>, IEFJournalsRepository
{
    private readonly MainDbContext _context;

    public JournalsRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ICollection<Journal>> GetByVehicleId(long id)
    {
        return await _context.Journals.Where(j => j.VehicleId == id).ToListAsync();
    }

    public async Task<ICollection<Journal>> ListAllAsync()
    {
        return await _context.Journals.ToListAsync();
    }

    public async Task<ICollection<Journal>> SearchAsync(int take = 10, int skip = 0)
    {
        return await _context.Journals
            .OrderBy(j => j.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Journals.CountAsync();
    }

    public async Task AddAsync(Journal journal)
    {
        _context.Journals.Add(journal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Journal journal)
    {
        _context.Journals.Update(journal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var journal = await _context.Journals.FindAsync(id);
        if (journal != null)
        {
            _context.Journals.Remove(journal);
            await _context.SaveChangesAsync();
        }
    }
}