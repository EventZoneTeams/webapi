using Microsoft.AspNetCore.Mvc;
using Repositories.Extensions;

namespace Services.DTO.EventCategoryDTOs
{
    public class CategoryParam
    {
        [BindProperty(Name = "search-term")]
        public string SearchTerm { get; set; }
        [BindProperty(Name = "order-by")]
        public EventCategoryOrderBy OrderBy { get; set; }
    }
}
