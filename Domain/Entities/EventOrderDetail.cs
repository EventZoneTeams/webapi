namespace EventZone.Domain.Entities
{
    public class EventOrderDetail : BaseEntity
    {
        /// <summary>
        /// public Guid PackageId { get; set; }
        /// </summary>

        public Guid EventProductId { get; set; }

        public Guid EventOrderId { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public bool? IsReceived { get; set; } = false;

        // public virtual EventPackage EventPackage { get; set; }
        public virtual EventProduct EventProduct { get; set; }

        public virtual EventOrder EventOrder { get; set; }
    }
}