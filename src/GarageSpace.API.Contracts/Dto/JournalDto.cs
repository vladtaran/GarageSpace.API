using GarageSpace.API.Contracts.Dto;
using GarageSpace.API.Contracts.Dto.Vehicle;

namespace GarageSpace.API.Contracts.Dto
{
    public class JournalDto : BaseAuditableEntityDto
    {
        public string Title { get; set; }
        public UserDto Owner { get; set; }
        public VehicleDto Vehicle { get; set; }

    }
}
