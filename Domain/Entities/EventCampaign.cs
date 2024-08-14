namespace EventZone.Domain.Entities
{
    public class EventCampaign : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public long GoalAmount { get; set; }
        public long CollectedAmount { get; set; }
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<EventDonation>? EventDonations { get; set; }
    }
}