namespace EventZone.Domain.Entities
{
    public class BookedTicket : BaseEntity
    {
        public Guid EventTicketId { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public Int64 PaidPrice { get; set; }
        public string AttendeeNote { get; set; }
        public bool IsCheckedIn { get; set; }
        public Guid EventOrderId { get; set; }
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual EventTicket EventTicket { get; set; }
        public virtual EventOrder EventOrder { get; set; }
    }
}