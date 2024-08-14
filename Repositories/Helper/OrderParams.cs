using EventZone.Domain.Enums;

namespace EventZone.Repositories.Helper
{
    public class OrderParams : PaginationParams
    {
        //public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public EventOrderStatusEnums? Status { get; set; }
    }
}
