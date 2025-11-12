using EficazAPI.Domain.Entities;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Domain.Services
{
    public interface ITaskDomainService
    {
        Task<List<TaskItem>> RecalculateAllPrioritiesAsync(List<TaskItem> tasks);

        bool CanMoveToStatus(TaskItem task, TaskStatus newStatus, User? user = null);

        Task<ProductivityMetrics> CalculateUserProductivityAsync(Guid userId, List<TaskItem> userTasks);
    }

    public class ProductivityMetrics
    {
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int TodoTasks { get; set; }
        public float AveragePriorityScore { get; set; }
        public TimeSpan AverageCompletionTime { get; set; }
    }
}
