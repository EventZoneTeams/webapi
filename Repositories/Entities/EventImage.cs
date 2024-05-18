namespace Repositories.Entities
{
    public class EventImage : BaseEntity
    {
        public int EventId { get; set; }
        public int PostId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }

        public virtual Event Event { get; set; }

        public virtual Post Post { get; set; }
    }
}
