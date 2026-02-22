using System.ComponentModel.DataAnnotations.Schema;

namespace GarageSpace.Models.Repository.EF.Vehicles
{
    [Table("Cars")]
    public class Car : MotorVehicle
    {
        public TransmitionTypes Transmission { get ; set; }
        public string? Trim { get; set; }
        public string? Body { get; set; }
        public int NumberOfDoors { get; set; }
        
    }

    public enum TransmitionTypes 
    {
        None,
        Manual,
        Automatic,
        CVT,
        Robotic
    }
}
