using AutoMapper;
using GarageSpace.Repository.Interfaces.EF;
using GarageSpace.Services.Interfaces;
using GarageSpace.API.Contracts.Dto;

namespace GarageSpace.Services
{
    public class JournalsService : IJournalsService
    {
        private readonly IEFJournalsRepository _repository;
        private readonly IMapper _mapper;
        public JournalsService(IEFJournalsRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<JournalDto>> GetByVehicleId(long id)
        {
            var result = await _repository.GetByVehicleId(id);

            return _mapper.Map<ICollection<JournalDto>>(result);
        }
    }
}
