﻿namespace EventZone.Domain.Entities
{
    public class Post : BaseEntity
    {
        public Guid EventId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public virtual Event Event { get; set; }
        public virtual ICollection<EventImage> EventImages { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
    }
}
