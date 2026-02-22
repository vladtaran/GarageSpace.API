using GarageSpace.API.Contracts.Dto;
using GarageSpace.API.Contracts.Dto.Vehicle;

namespace GarageSpace.API.Contracts.Dto
{
    public class GarageDto : BaseAuditableEntityDto
    {
        public UserDto Owner { get; set; }

        public IList<CarDto> Cars { get; set; }
        public IList<MotocycleDto> Motorcycles { get; set; }
        public IList<TrailerDto> Trailers { get; set; }

    }
}
