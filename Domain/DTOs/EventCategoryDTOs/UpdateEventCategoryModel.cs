using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.DTOs.EventCategoryDTOs
{
    public class UpdateEventCategoryModel
    {
        [BindProperty(Name = "title")]
        public string Title { get; set; }
        [BindProperty(Name = "image")]
        public IFormFile? Image { get; set; }
    }
}
