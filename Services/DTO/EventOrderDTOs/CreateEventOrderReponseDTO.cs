namespace Services.DTO.EventOrderDTOs
{
    public class CreateEventOrderReponseDTO
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public List<CreateEventOrderDetailsReponseDTO> EventOrderDetails { get; set; }
    }
}
