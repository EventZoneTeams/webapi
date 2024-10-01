namespace EventZone.Domain.Entities
{
    public class EventPackage : BaseEntity
    {
        public Guid EventId { get; set; }
        public string? Title { get; set; }

        public string? ThumbnailUrl { get; set; }
        public long TotalPrice { get; set; }
        public string? Description { get; set; }

        public virtual Event Event { get; set; }

        //  public virtual ICollection<EventOrderDetail> EventOrderDetails { get; set; }
        public virtual ICollection<ProductInPackage> ProductsInPackage { get; set; }
    }
}