namespace EventZone.Domain.Entities
{
    public class EventTicket : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid EventId { get; set; }
        public int InStock { get; set; }
        public Int64 Price { get; set; }
        public virtual Event Event { get; set; }
    }
}
