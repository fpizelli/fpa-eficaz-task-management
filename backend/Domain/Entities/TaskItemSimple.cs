namespace EficazAPI.Domain.Entities
{
    public class TaskItemSimple
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; }
        public Guid? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public int Impact { get; set; }
        public int Effort { get; set; }
        public int Urgency { get; set; }
        public float Score { get; set; }
        
        public UserSimple? User { get; set; }
    }
}
