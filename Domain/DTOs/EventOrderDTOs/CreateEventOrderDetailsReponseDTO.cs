namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class CreateEventOrderDetailsReponseDTO
    {
        public Guid EventProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}