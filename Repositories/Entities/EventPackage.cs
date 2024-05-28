namespace Repositories.Entities
{
    public class EventPackage : BaseEntity
    {
        public int EventId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int TotalPrice { get; set; }
        public string Description { get; set; }

        public virtual Event Event { get; set; }
        public virtual ICollection<EventOrderDetail> EventOrderDetails { get; set; }
        public virtual ICollection<ProductInPackage> ProductsInPackage { get; set; }
    }

}
