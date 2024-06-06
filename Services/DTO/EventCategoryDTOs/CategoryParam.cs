using Repositories.Extensions;

namespace Services.DTO.EventCategoryDTOs
{
    public class CategoryParam
    {
        public string SearchTerm { get; set; }
        public EventCategoryOrderBy OrderBy { get; set; }
    }
}
