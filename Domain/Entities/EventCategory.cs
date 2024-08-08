namespace Domain.Entities
{
    public class EventCategory : BaseEntity
    {
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
