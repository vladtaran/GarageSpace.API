using GarageSpaceAPI.Contracts.Dto.Vehicle;
namespace GarageSpace.Services.Interfaces;

public interface ICarsService
{
    public Task<IList<CarDto>> Search(int take, int skip);
    Task Update(long id, CarDto carDto);
    Task<long> Create(CarDto car);
    Task Delete(long id);
    Task<IList<CarDto>> GetByUserId(long id);
}