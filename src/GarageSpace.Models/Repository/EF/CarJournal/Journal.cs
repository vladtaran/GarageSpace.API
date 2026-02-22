using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GarageSpace.Models.Repository.EF;
using GarageSpace.Models.Repository.EF.Vehicles;

namespace GarageSpace.Models.Repository.EF.CarJournal
{
    [Table("Journals")]
    public class Journal : BaseAuditableEntity
    {
        [Required]
        public long VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public long CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public string Title { get; set; }

        public ICollection<JournalRecord> JournalRecords { get; set; } = new List<JournalRecord>();
    }
}