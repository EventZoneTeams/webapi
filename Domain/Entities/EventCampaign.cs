namespace Domain.Entities
{
    public class EventCampaign : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public Int64 GoalAmount { get; set; }
        public Int64 CollectedAmount { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
