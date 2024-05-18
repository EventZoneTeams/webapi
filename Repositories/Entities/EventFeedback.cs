namespace Repositories.Entities
{
    public class EventFeedback : BaseEntity
    {
        public int EventId { get; set; }
        public string Content { get; set; }

        public virtual Event Event { get; set; }
    }
}
