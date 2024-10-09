namespace EventZone.Domain.Entities
{
    public class EventStaff : BaseEntity
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string? Note { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
