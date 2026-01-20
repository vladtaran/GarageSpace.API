using AutoMapper;
using GarageSpaceAPI.Contracts.Dto;
using GarageSpace.Repository.Interfaces.EF;
using GarageSpace.Services.Interfaces;

namespace GarageSpace.Services
{
    public class GarageService : IGarageService
    {
        private readonly IEFGaragesRepository _repository;
        private readonly IMapper _mapper;
        public GarageService(IEFGaragesRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IList<GarageDto>> Search(int take, int skip)
        {
            var result = await _repository.SearchAsync(take, skip);

            return _mapper.Map<IList<GarageDto>>(result);
        }

        public async Task<GarageDto?> GetGarageByOwner(long ownerId)
        {
            var result = await _repository.GetByOwnerIdAsync(ownerId);
            if (result == null) 
            {
                return null;
            }

            return _mapper.Map<GarageDto>(result);
        }
    }
}
