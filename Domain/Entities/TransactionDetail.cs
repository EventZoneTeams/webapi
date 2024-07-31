namespace Domain.Entities
{
    public class TransactionDetail : BaseEntity
    {
        public Guid TransactionId { get; set; }
        public Guid EventOrderId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual EventOrder EventOrder { get; set; }
    }
}
