namespace Repositories.Entities
{
    public class PostComment : BaseEntity
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
