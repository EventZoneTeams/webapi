namespace Repositories.Commons.Payload.RequestModels
{
    public class EventParams : PaginationParameter
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public int? EventCategoryId { get; set; }
    }
}
