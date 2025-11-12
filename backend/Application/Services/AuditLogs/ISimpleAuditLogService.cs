namespace EficazAPI.Application.Services.AuditLogs
{
    public interface ISimpleAuditLogService
    {
        Task LogTaskCreatedAsync(Guid taskId, string title, Guid? userId, float priorityScore);
        Task LogTaskUpdatedAsync(Guid taskId, string changes, Guid? userId);
        Task LogTaskDeletedAsync(Guid taskId, string title, Guid? userId);
        Task LogTaskStatusChangedAsync(Guid taskId, string oldStatus, string newStatus, Guid? userId);
    }
}
