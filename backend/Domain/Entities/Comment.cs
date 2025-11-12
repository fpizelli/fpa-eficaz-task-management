using EficazAPI.Domain.Events;

namespace EficazAPI.Domain.Entities
{
    public class Comment
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
        public TaskItem Task { get; set; } = null!;

        public Comment()
        {
        }

        public Comment(Guid taskId, Guid userId, string content)
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("TaskId cannot be empty", nameof(taskId));
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId cannot be empty", nameof(userId));
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Content cannot be empty", nameof(content));

            Id = Guid.NewGuid();
            TaskId = taskId;
            UserId = userId;
            Content = content.Trim();
            CreatedAt = DateTime.UtcNow;

            _domainEvents.Add(new CommentAddedEvent(Id, TaskId, UserId, Content));
        }

        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("Content cannot be empty", nameof(newContent));

            Content = newContent.Trim();
        }

        public bool IsFromUser(Guid userId)
        {
            return UserId == userId;
        }

        public bool IsForTask(Guid taskId)
        {
            return TaskId == taskId;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Content);
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
