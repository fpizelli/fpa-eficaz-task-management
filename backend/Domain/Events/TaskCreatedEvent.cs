namespace EficazAPI.Domain.Events
{
    public sealed class TaskCreatedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
        public Guid TaskId { get; }
        public string Title { get; }
        public Guid? UserId { get; }
        public float PriorityScore { get; }

        public TaskCreatedEvent(Guid taskId, string title, Guid? userId, float priorityScore)
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            TaskId = taskId;
            Title = title;
            UserId = userId;
            PriorityScore = priorityScore;
        }
    }
}
