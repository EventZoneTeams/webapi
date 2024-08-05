using Domain.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Domain.DTOs.EventCategoryDTOs
{
    public class CategoryParam
    {
        [BindProperty(Name = "search-term")]
        public string SearchTerm { get; set; }

        [BindProperty(Name = "order-by")]
        public EventCategoryOrderBy OrderBy { get; set; }
    }
}