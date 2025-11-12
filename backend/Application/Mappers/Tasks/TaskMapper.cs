using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Mappers.Tasks
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(TaskItem task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Impact = task.Priority.Impact,
                Effort = task.Priority.Effort,
                Urgency = task.Priority.Urgency,
                PriorityScore = task.Priority.Score,
                Status = task.Status,
                UserName = task.User?.Name,
                CreatedAt = task.CreatedAt
            };
        }

        public static List<TaskDto> ToDto(IEnumerable<TaskItem> tasks)
        {
            if (tasks == null)
                throw new ArgumentNullException(nameof(tasks));

            return tasks.Select(ToDto).ToList();
        }
    }
}
