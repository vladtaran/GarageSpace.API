using System.ComponentModel.DataAnnotations.Schema;

namespace GarageSpace.Models.Repository.EF
{
    [Table("Manufacturers")]
    public class Manufacturer
    {
        public long Id { get; set; }
        public ManufacturerEnum ManufacturerName { get; set; }
        public long YearCreation { get; set; }

        public int ManufacturerCountryId { get; set; }
        public Country? ManufacturerCountry { get; set; }

    }
}
