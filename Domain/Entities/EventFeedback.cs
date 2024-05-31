namespace Domain.Entities
{
    public class EventFeedback : BaseEntity
    {
        public int EventId { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }

        public virtual Event Event { get; set; }

        public virtual User User { get; set; }
    }
}
