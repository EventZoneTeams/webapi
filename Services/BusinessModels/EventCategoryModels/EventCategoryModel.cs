using Domain.Entities;

namespace Services.BusinessModels.EventCategoryModels
{
    public class EventCategoryModel : BaseEntity
    {
        public string Title { get; set; }
        public string? ImageUrl { get; set; }

    }
}
