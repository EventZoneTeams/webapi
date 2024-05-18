namespace Repositories.Entities
{
    public class EventOrder : BaseEntity
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EventOrderDetail> EventOrderDetails { get; set; }
        public virtual ICollection<OrderTransaction> OrderTransactions { get; set; }
    }
}
