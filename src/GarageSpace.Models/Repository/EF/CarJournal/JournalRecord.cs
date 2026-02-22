using GarageSpace.Models.Repository.EF;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageSpace.Models.Repository.EF.CarJournal
{
    [Table("JournalRecords")]
    public class JournalRecord : BaseAuditableEntity
    {
        [Required]
        public long JournalId { get; set; }

        public Journal Journal { get; set; }  // Navigation to Journal

        public DateTime EntryDate { get; set; }
        public string Title { get; set; }     // e.g., "Oil Change", "New Tires"
        public string Description { get; set; }  // Detailed notes

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Cost { get; set; }    // Optional cost of the work
        public int? Mileage { get; set; }     // Optional mileage at time of entry
    }
}
