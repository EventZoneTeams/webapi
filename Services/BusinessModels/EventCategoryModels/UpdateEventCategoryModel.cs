using Microsoft.AspNetCore.Http;

namespace Services.BusinessModels.EventCategoryModels
{
    public class UpdateEventCategoryModel
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
    }
}
