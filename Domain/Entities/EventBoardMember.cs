namespace EventZone.Domain.Entities
{
    public class EventBoardMember
    {
        public Guid EventBoardId { get; set; }
        public Guid UserId { get; set; }
        public virtual EventBoard EventBoard { get; set; }
        public virtual User User { get; set; }
    }
}
