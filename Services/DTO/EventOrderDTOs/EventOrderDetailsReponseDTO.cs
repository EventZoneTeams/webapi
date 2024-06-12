namespace Services.DTO.EventOrderDTOs
{
    public class EventOrderDetailsReponseDTO
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public int EventOrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
