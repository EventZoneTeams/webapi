namespace Repositories.Entities
{
    public class ProductInPackage
    {
        public int ProductId { get; set; }
        public int PackageId { get; set; }
        public int Quantity { get; set; }

        public virtual EventProduct EventProduct { get; set; }
        public virtual EventPackage EventPackage { get; set; }
    }
}
