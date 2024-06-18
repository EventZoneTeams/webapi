using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Repositories.Helper
{
    public class EventParams : PaginationParams
    {
        //public string? OrderBy { get; set; }
        [BindProperty(Name = "search-term")]
        public string? SearchTerm { get; set; }
        [BindProperty(Name = "event-category-id")]
        public int? EventCategoryId { get; set; }
        [BindProperty(Name = "is-donation")]
        public bool? IsDonation { get; set; }
        [BindProperty(Name = "user-id")]
        public int? UserId { get; set; }
        [BindProperty(Name = "donation-start-date")]
        public DateTime? DonationStartDate { get; set; }
        [BindProperty(Name = "donation-end-date")]
        public DateTime? DonationEndDate { get; set; }
        [BindProperty(Name = "event-start-date")]
        public DateTime? EventStartDate { get; set; }
        [BindProperty(Name = "event-end-date")]
        public DateTime? EventEndDate { get; set; }
        [BindProperty(Name = "status")]
        public EventStatusEnums? Status { get; set; }
        [BindProperty(Name = "origanization-status-enums")]
        public OriganizationStatusEnums? OriganizationStatusEnums { get; set; }
    }
}
