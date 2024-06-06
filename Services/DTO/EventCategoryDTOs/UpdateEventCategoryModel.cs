using Microsoft.AspNetCore.Http;

namespace Services.DTO.EventCategoryDTOs
{
    public class UpdateEventCategoryModel
    {
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
    }
}
