namespace Domain.Entities
{
    public class EventProduct : BaseEntity
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }

        public virtual Event Event { get; set; }
        public virtual ICollection<ProductInPackage> ProductsInPackage { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
