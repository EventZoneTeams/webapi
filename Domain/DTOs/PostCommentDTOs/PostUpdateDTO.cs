using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.PostCommentDTOs
{
    public class PostUpdateDTO
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public List<string>? ImageUrls { get; set; }
    }
}