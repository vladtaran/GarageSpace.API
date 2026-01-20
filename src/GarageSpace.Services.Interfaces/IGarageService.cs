using GarageSpaceAPI.Contracts.Dto;

namespace GarageSpace.Services.Interfaces;

public interface IGarageService
{
    public Task<IList<GarageDto>> Search(int take, int skip);
    public Task<GarageDto?> GetGarageByOwner(long ownerId);
    
}