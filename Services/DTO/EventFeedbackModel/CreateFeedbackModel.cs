namespace Services.DTO.EventFeedbackModel
{
    public class CreateFeedbackModel
    {
        public Guid EventId { get; set; }
        public string Content { get; set; }
        //public int UserId { get; set; }
    }
}