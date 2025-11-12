using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Services.AuditLogs
{
    public interface IAuditService
    {
        Task LogTaskChangesAsync(Guid taskId, TaskItem originalTask, TaskItem updatedTask);

        Task LogActionAsync(Guid taskId, string action, string? oldValue, string? newValue);
    }
}
