namespace EficazAPI.Domain.Events
{
    public sealed class CommentAddedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
        public Guid CommentId { get; }
        public Guid TaskId { get; }
        public Guid UserId { get; }
        public string Content { get; }

        public CommentAddedEvent(Guid commentId, Guid taskId, Guid userId, string content)
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            CommentId = commentId;
            TaskId = taskId;
            UserId = userId;
            Content = content;
        }
    }
}
