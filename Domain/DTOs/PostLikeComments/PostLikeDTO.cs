using EventZone.Domain.DTOs.PostCommentDTOs;
using EventZone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventZone.Domain.DTOs.PostLikeComments
{
    public class PostLikeDTO : PostLikeUpdateDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public virtual PostDTO? Post { get; set; }
    }
}