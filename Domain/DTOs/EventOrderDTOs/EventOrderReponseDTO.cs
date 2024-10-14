using EventZone.Domain.DTOs.BookedTicketDTOs;

namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class EventOrderReponseDTO
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public long TotalAmount { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public List<BookedTicketDTO>? BookedTickets { get; set; }

        public List<EventOrderDetailsReponseDTO>? EventOrderDetails { get; set; }
    }
}