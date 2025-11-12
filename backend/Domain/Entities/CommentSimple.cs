namespace EficazAPI.Domain.Entities
{
    public class CommentSimple
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        public TaskItemSimple Task { get; set; } = null!;
        public UserSimple User { get; set; } = null!;
    }
}
