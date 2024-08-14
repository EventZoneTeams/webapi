namespace EventZone.Domain.DTOs.EventOrderDTOs
{
    public class CreateEventOrderReponseDTO
    {
        public Guid EventId { get; set; }
        public List<CreateEventOrderDetailsReponseDTO> EventOrderDetails { get; set; }
    }
}