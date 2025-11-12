using EficazAPI.Application.DTOs.Tasks;

namespace EficazAPI.Application.Commands.Tasks
{
    public class CreateTaskCommand : ICommand<TaskDto>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Impact { get; set; } = 5;
        public int Effort { get; set; } = 5;
        public int Urgency { get; set; } = 5;
        public string? UserId { get; set; }
    }
}
