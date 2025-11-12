using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Domain.Events
{
    public sealed class TaskStatusChangedEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOn { get; }
        public Guid TaskId { get; }
        public TaskStatus OldStatus { get; }
        public TaskStatus NewStatus { get; }
        public Guid? UserId { get; }

        public TaskStatusChangedEvent(Guid taskId, TaskStatus oldStatus, TaskStatus newStatus, Guid? userId)
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
            TaskId = taskId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            UserId = userId;
        }
    }
}
