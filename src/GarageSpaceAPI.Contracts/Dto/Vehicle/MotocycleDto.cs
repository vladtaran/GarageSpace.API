namespace GarageSpaceAPI.Contracts.Dto.Vehicle
{
    public class MotocycleDto : MotorVehicleDto
    {
        public bool HasSideCar { get; set; }
        public string? Type { get; set; }
        
    }
}
