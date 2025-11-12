using EficazAPI.Domain.ValueObjects;
using EficazAPI.Domain.Events;

namespace EficazAPI.Domain.Entities
{
    public class TaskItem
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskPriority Priority { get; set; } = null!;
        public int Impact { get; set; }
        public int Effort { get; set; }
        public int Urgency { get; set; }
        public float Score { get; set; }
        public TaskStatus Status { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public User? User { get; set; }

        public TaskItem()
        {
            _domainEvents = new List<IDomainEvent>();
        }

        public TaskItem(string title, string? description, TaskPriority priority, Guid? userId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Priority = priority ?? throw new ArgumentNullException(nameof(priority));
            Impact = priority.Impact;
            Effort = priority.Effort;
            Urgency = priority.Urgency;
            Score = priority.Score;
            UserId = userId;
            Status = TaskStatus.Todo;
            CreatedAt = DateTime.UtcNow;

            _domainEvents.Add(new TaskCreatedEvent(Id, Title, UserId, Priority.Score));
        }

        public void UpdateDetails(string title, string? description)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            Title = title;
            Description = description;
        }

        public void UpdatePriority(TaskPriority newPriority)
        {
            Priority = newPriority ?? throw new ArgumentNullException(nameof(newPriority));
            Impact = newPriority.Impact;
            Effort = newPriority.Effort;
            Urgency = newPriority.Urgency;
            Score = newPriority.Score;
        }

        public void UpdatePriority(int impact, int effort, int urgency)
        {
            Priority = TaskPriority.Create(impact, effort, urgency);
            Impact = Priority.Impact;
            Effort = Priority.Effort;
            Urgency = Priority.Urgency;
            Score = Priority.Score;
        }

        public void ChangeStatus(TaskStatus newStatus, Guid? userId = null)
        {
            if (Status == newStatus) return;

            var oldStatus = Status;
            Status = newStatus;

            _domainEvents.Add(new TaskStatusChangedEvent(Id, oldStatus, newStatus, userId));
        }

        public void AssignToUser(Guid userId)
        {
            UserId = userId;
        }

        public void UnassignUser()
        {
            UserId = null;
        }

        public bool IsAssignedTo(Guid userId)
        {
            return UserId == userId;
        }

        public bool IsCompleted()
        {
            return Status == TaskStatus.Done;
        }

        public bool IsInProgress()
        {
            return Status == TaskStatus.InProgress;
        }

        public bool IsTodo()
        {
            return Status == TaskStatus.Todo;
        }

        public bool HasHighPriority()
        {
            return Priority.Score >= 8.0f;
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        internal void SetStatusForSeeding(TaskStatus status)
        {
            Status = status;
        }
    }
}
