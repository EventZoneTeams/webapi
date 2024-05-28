using Domain.Enums;

namespace Repositories.Helper
{
    public class EventParams : PaginationParams
    {
        //public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public int? EventCategoryId { get; set; }
        public bool? IsDonation { get; set; }
        public DateTime? DonationStartDate { get; set; }
        public DateTime? DonationEndDate { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string? Location { get; set; }
        public string? University { get; set; }
        public EventStatusEnums? Status { get; set; }
        public OriganizationStatusEnums? OriganizationStatusEnums { get; set; }
    }
}
