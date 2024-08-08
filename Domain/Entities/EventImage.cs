namespace Domain.Entities
{
    public class EventImage : BaseEntity
    {
        public Guid EventId { get; set; }
        public Guid? PostId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public virtual Event Event { get; set; }
        public virtual Post? Post { get; set; }
    }
}
