using GarageSpace.API.Contracts.Dto;

namespace GarageSpace.API.Contracts.Dto.Vehicle
{
    public class VehicleDto : BaseAuditableEntityDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public float? Weight { get; set; }
        public string? VIN { get; set; }
        public string LicensePlate { get; set; }
        public ManufacturerDto Manufacturer { get; set; }
    }
}
