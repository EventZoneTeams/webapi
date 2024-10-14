namespace EventZone.Domain.Entities
{
    public class PostLike : BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
