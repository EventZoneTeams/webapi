using Repositories.Extensions;

namespace Services.BusinessModels.EventCategoryModels
{
    public class CategoryParam
    {
        public string SearchTerm { get; set; }
        public EventCategoryOrderBy OrderBy { get; set; }
    }
}
