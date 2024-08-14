namespace EventZone.Domain.Entities
{
    public class ProductInPackage
    {
        public Guid ProductId { get; set; }
        public Guid PackageId { get; set; }
        public int Quantity { get; set; }

        public virtual EventProduct EventProduct { get; set; }
        public virtual EventPackage EventPackage { get; set; }
    }
}