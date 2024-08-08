using Domain.Enums;

namespace Repositories.Helper
{
    public class EventParams : PaginationParams
    {
        //public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public Guid? EventCategoryId { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public EventStatusEnums? Status { get; set; }
    }
}
