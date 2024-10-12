using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.PostCommentDTOs
{
    public class PostDTO
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}