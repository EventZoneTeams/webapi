namespace Domain.Entities
{
    public class PostComment : BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
