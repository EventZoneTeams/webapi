namespace EventZone.Domain.Entities
{
    public class EventOrderDetail : BaseEntity
    {
        public Guid PackageId { get; set; }
        public Guid EventOrderId { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }

        public virtual EventPackage EventPackage { get; set; }
        public virtual EventOrder EventOrder { get; set; }
    }
}
