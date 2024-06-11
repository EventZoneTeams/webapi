namespace Services.DTO.EventOrderDTOs
{
    public class EventOrderReponseDTO
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

    }
}
