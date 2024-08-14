namespace EventZone.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public virtual EventProduct EventProduct { get; set; }
    }
}