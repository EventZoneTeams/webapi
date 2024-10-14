using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.PostCommentDTOs
{
    public class PostDetailDTO : PostDTO
    {
        public Guid Id { get; set; }
        public virtual EventDTO? Event { get; set; }
        public virtual ICollection<PostCommentDTO>? PostComments { get; set; }
    }
}