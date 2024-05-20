using Microsoft.AspNetCore.Mvc;

namespace Repositories.Commons.Payload.Requests
{
    public class EventParams : PaginationParameter
    {
        [FromQuery]
        public string OrderBy { get; set; }
        [FromQuery]
        public string SearchTerm { get; set; }
    }
}
