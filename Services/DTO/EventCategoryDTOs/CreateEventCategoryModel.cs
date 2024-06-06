using Microsoft.AspNetCore.Http;

namespace Services.DTO.EventCategoryDTOs
{
    public class CreateEventCategoryModel
    {
        public string Title { get; set; }
        public IFormFile Image { get; set; }
    }
}
