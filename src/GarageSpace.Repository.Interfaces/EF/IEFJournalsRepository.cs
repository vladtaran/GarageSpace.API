using GarageSpace.Data.Models.EF.CarJournal;

namespace GarageSpace.Repository.Interfaces.EF;

public interface IEFJournalsRepository : IBaseRepository
{
    Task<ICollection<Journal>> GetByVehicleId(long id);
    Task<ICollection<Journal>> ListAllAsync();
    Task<ICollection<Journal>> SearchAsync(int take = 10, int skip = 0);
    Task<int> GetTotalCountAsync();
    Task AddAsync(Journal journal);
    Task UpdateAsync(Journal journal);
    Task DeleteAsync(long id);
}
