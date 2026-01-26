using GarageSpaceAPI.Contracts.Dto;

namespace GarageSpace.Services.Interfaces;

public interface IJournalsService
{
    Task<ICollection<JournalDto>> GetByVehicleId(long id);
}