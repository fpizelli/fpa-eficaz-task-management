using EficazAPI.Domain.Entities;
using EficazAPI.Application.Ports.Shared;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Application.Ports.Tasks
{
    public interface ITaskQueryRepository : IQueryRepository<TaskItem, Guid>
    {
        Task<List<TaskItem>> GetByUserIdAsync(Guid userId);
        Task<List<TaskItem>> GetByStatusAsync(TaskStatus status);
        Task<List<TaskItem>> GetHighPriorityTasksAsync(float minPriorityScore = 8.0f);
        Task<List<TaskItem>> GetOverdueTasksAsync(int daysOld = 7);
    }

    public interface ITaskCommandRepository : ICommandRepository<TaskItem, Guid>
    {
        Task<TaskItem> AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
    }

    /// </summary>
    public interface ITaskRepository : ITaskQueryRepository, ITaskCommandRepository
    {
        // Métodos síncronos mantidos para compatibilidade
        List<TaskItem> GetAll();
        TaskItem? GetById(Guid id);
        List<TaskItem> GetByUserId(Guid userId);
        List<TaskItem> GetByStatus(TaskStatus status);
    }
}
