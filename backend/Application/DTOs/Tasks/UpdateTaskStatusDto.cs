namespace EficazAPI.Application.DTOs.Tasks
{
    public class UpdateTaskStatusDto
    {
        public Guid TaskId { get; set; }
        public EficazAPI.Domain.Entities.TaskStatus Status { get; set; }
    }
}
