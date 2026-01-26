using GarageSpaceAPI.Contracts.Dto.Vehicle;

namespace GarageSpaceAPI.Contracts.Dto
{
    public class JournalDto : BaseAuditableEntityDto
    {
        public string Title { get; set; }
        public UserDto Owner { get; set; }
        public VehicleDto Vehicle { get; set; }

    }
}
