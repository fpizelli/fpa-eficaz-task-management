using EficazAPI.Domain.Entities;
using EficazAPI.Domain.Enums;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Domain.Services
{
    public class TaskDomainService : ITaskDomainService
    {
        public async Task<List<TaskItem>> RecalculateAllPrioritiesAsync(List<TaskItem> tasks)
        {
            if (tasks == null || !tasks.Any())
                return new List<TaskItem>();

            foreach (var task in tasks)
            {
                if (task.IsInProgress())
                {
                    var currentPriority = task.Priority;
                    var boostedUrgency = Math.Min(10, currentPriority.Urgency + 1);
                    task.UpdatePriority(currentPriority.Impact, currentPriority.Effort, boostedUrgency);
                }

                var daysSinceCreation = (DateTime.UtcNow - task.CreatedAt).Days;
                if (daysSinceCreation > 7)
                {
                    var currentPriority = task.Priority;
                    var ageBoost = Math.Min(2, daysSinceCreation / 7);
                    var newUrgency = Math.Min(10, currentPriority.Urgency + ageBoost);
                    task.UpdatePriority(currentPriority.Impact, currentPriority.Effort, newUrgency);
                }
            }

            return tasks.OrderByDescending(t => t.Priority.Score).ToList();
        }

        public bool CanMoveToStatus(TaskItem task, TaskStatus newStatus, User? user = null)
        {
            if (task == null)
                return false;

            if (newStatus == TaskStatus.InProgress)
            {
                if (task.UserId.HasValue && user != null)
                {
                    return task.IsAssignedTo(user.Id) || user.IsAdmin();
                }
                return user?.IsAdmin() == true;
            }

            if (newStatus == TaskStatus.Done && task.HasHighPriority())
            {
                return user?.IsAdmin() == true || user?.IsManager() == true;
            }

            if (task.IsCompleted() && newStatus != TaskStatus.Done)
            {
                return user?.IsAdmin() == true;
            }

            return true;
        }

        public async Task<ProductivityMetrics> CalculateUserProductivityAsync(Guid userId, List<TaskItem> userTasks)
        {
            if (userTasks == null || !userTasks.Any())
            {
                return new ProductivityMetrics();
            }

            var completedTasks = userTasks.Where(t => t.IsCompleted()).ToList();
            var inProgressTasks = userTasks.Where(t => t.IsInProgress()).ToList();
            var todoTasks = userTasks.Where(t => t.IsTodo()).ToList();

            var averagePriorityScore = userTasks.Any() 
                ? userTasks.Average(t => t.Priority.Score) 
                : 0f;

            var averageCompletionTime = completedTasks.Any()
                ? TimeSpan.FromDays(completedTasks.Average(t => (DateTime.UtcNow - t.CreatedAt).TotalDays))
                : TimeSpan.Zero;

            return await Task.FromResult(new ProductivityMetrics
            {
                CompletedTasks = completedTasks.Count,
                InProgressTasks = inProgressTasks.Count,
                TodoTasks = todoTasks.Count,
                AveragePriorityScore = averagePriorityScore,
                AverageCompletionTime = averageCompletionTime
            });
        }
    }
}
