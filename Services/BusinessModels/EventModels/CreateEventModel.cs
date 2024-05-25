namespace Services.BusinessModels.EventModels
{
    public class CreateEventModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DonationStartDate { get; set; }
        public DateTime? DonationEndDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Location { get; set; }
        public int UserId { get; set; }
        public string? University { get; set; }
        public string Status { get; set; } = "0";
        public string? OriganizationStatus { get; set; }
        public bool IsDonation { get; set; }
        public decimal TotalCost { get; set; }
    }
}
