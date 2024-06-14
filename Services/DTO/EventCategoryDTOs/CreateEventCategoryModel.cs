﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services.DTO.EventCategoryDTOs
{
    public class CreateEventCategoryModel
    {
        [BindProperty(Name = "title")]
        public string Title { get; set; }
        [BindProperty(Name = "image")]
        public IFormFile Image { get; set; }
    }
}
