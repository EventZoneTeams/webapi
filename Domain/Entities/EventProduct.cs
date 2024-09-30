namespace EventZone.Domain.Entities
{
    public class EventProduct : BaseEntity
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int QuantityInStock { get; set; }

        public virtual Event Event { get; set; }
        public virtual ICollection<ProductInPackage> ProductsInPackage { get; set; }
        public virtual ICollection<EventOrderDetail> EventOrderDetails { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}