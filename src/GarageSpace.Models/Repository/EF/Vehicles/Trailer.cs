using System.ComponentModel.DataAnnotations.Schema;

namespace GarageSpace.Models.Repository.EF.Vehicles
{
    [Table("Trailers")]
    public class Trailer : Vehicle
    {
        public float? LoadCapacityKg { get; set; }     // Maximum load in kilograms
        public int? NumberOfAxles { get; set; }        // E.g., 1 or 2
        public string? TrailerType { get; set; }       // E.g., Flatbed, Enclosed, Utility
        public bool HasBrakes { get; set; }            // Whether the trailer has its own brake system
        public float? LengthMeters { get; set; }       // Optional for longer trailers
    }
}
