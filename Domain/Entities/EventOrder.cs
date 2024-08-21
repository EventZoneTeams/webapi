namespace EventZone.Domain.Entities
{
    public class EventOrder : BaseEntity
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public string OrderType { get; set; } //PRODUCT OR TICKET
        public long TotalAmount { get; set; }
        public string Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EventOrderDetail> EventOrderDetails { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } // thieu s nho fix
        public virtual ICollection<BookedTicket>? BookedTickets { get; set; }
    }
}