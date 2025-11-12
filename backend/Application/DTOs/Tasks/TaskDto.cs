using EficazAPI.Application.DTOs.Tasks;
namespace EficazAPI.Application.DTOs.Tasks
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Impact { get; set; }
        public int Effort { get; set; }
        public int Urgency { get; set; }
        public float PriorityScore { get; set; }
        public EficazAPI.Domain.Entities.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
    }
}
