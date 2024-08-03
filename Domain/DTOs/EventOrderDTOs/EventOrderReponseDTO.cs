namespace Domain.DTOs.EventOrderDTOs
{
    public class EventOrderReponseDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public long TotalAmount { get; set; }
        public string Status { get; set; }
        public List<EventOrderDetailsReponseDTO> EventOrderDetails { get; set; }

    }
}
