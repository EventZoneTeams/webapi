namespace Domain.Entities
{
    public class TransactionDetail : BaseEntity
    {
        public int TransactionId { get; set; }
        public int EventOrderId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public virtual EventOrder EventOrder { get; set; }
    }
}
