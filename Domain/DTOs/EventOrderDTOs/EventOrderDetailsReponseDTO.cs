namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class EventOrderDetailsReponseDTO
    {
        public Guid Id { get; set; }
        public Guid EventProductId { get; set; }
        public Guid EventOrderId { get; set; }
        public int Quantity { get; set; }
        public long Price { get; set; }
        public bool? IsReceived { get; set; }
    }
}