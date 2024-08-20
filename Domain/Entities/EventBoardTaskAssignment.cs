namespace EventZone.Domain.Entities
{
    public class EventBoardTaskAssignment
    {
        public Guid EventBoardTaskId { get; set; }
        public Guid UserId { get; set; }
        public virtual EventBoardTask EventBoardTask { get; set; }
        public virtual User User { get; set; }
    }
}
