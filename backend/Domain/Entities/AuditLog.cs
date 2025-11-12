using EficazAPI.Domain.ValueObjects;

namespace EficazAPI.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public AuditAction Action { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public TaskItem Task { get; set; } = null!;
        public User? User { get; set; }

        public AuditLog()
        {
        }

        public AuditLog(Guid taskId, AuditAction action, string? oldValue = null, string? newValue = null, Guid? userId = null)
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("TaskId cannot be empty", nameof(taskId));

            Id = Guid.NewGuid();
            TaskId = taskId;
            Action = action ?? throw new ArgumentNullException(nameof(action));
            OldValue = oldValue;
            NewValue = newValue;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsForTask(Guid taskId)
        {
            return TaskId == taskId;
        }

        public bool IsFromUser(Guid userId)
        {
            return UserId == userId;
        }

        public bool HasValueChange()
        {
            return OldValue != NewValue;
        }

        public bool IsCreationAction()
        {
            return Action.IsCreation();
        }

        public bool IsUpdateAction()
        {
            return Action.IsUpdate();
        }

        public bool IsDeletionAction()
        {
            return Action.IsDeletion();
        }
    }
}
