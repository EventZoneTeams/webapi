namespace Repositories.Commons.Payload.Requests
{
    public class EventParams : PaginationParameter
    {
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
    }
}
