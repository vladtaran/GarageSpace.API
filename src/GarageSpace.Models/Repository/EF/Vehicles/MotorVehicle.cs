namespace GarageSpace.Models.Repository.EF.Vehicles
{
    public abstract class MotorVehicle : Vehicle
    {
        public string? Engine { get; set; }
        public int? HorsePower { get; set; }
        public string? Transmission { get; set; }
        public FuelTypes FuelType { get; set; }
    }

    public enum FuelTypes 
    {
        None = 0,
        Gasoline,
        Disel,
        Electro,
        LPG,
        LPGAndGasoline
    }
}
