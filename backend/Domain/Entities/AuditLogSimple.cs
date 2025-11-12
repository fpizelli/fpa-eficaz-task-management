namespace EficazAPI.Domain.Entities
{
    public class AuditLogSimple
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public TaskItemSimple Task { get; set; } = null!;
        public UserSimple? User { get; set; }
    }
}
