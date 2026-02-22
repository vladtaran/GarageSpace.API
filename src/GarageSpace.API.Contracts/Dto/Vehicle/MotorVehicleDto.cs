using GarageSpace.API.Contracts.Dto.Vehicle;

namespace GarageSpace.API.Contracts.Dto.Vehicle
{
    public class MotorVehicleDto : VehicleDto
    {
        public string? Engine { get; set; }
        public int? HorsePower { get; set; }
        public string? Transmission { get; set; }
    }
}
