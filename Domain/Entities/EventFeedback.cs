namespace Domain.Entities
{
    public class EventFeedback : BaseEntity
    {
        public Guid EventId { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
    }
}
