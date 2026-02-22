using GarageSpace.API.Contracts.Dto;

namespace GarageSpace.API.Contracts.Dto
{
    public class JournalRecordDto : BaseAuditableEntityDto
    {
        public DateTime EntryDate { get; set; }
        public string Title { get; set; }     // e.g., "Oil Change", "New Tires"
        public string Description { get; set; }  // Detailed notes
        public decimal? Cost { get; set; }    // Optional cost of the work
        public int? Mileage { get; set; }     // Optional mileage at time of entry

    }
}
