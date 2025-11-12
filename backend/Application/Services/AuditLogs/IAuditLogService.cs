using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.Enums;

namespace EficazAPI.Application.Services.AuditLogs
{
    public interface IAuditLogService
    {
        #region Priority Change Auditing
        Task LogPriorityChangeAsync(PriorityChangeAuditDto auditData);

        Task LogPriorityComponentsChangeAsync(
            Guid taskId,
            int? oldImpact, int? newImpact,
            int? oldEffort, int? newEffort,
            int? oldUrgency, int? newUrgency,
            Guid? userId = null,
            string? reason = null);

        #endregion

        #region Status Change Auditing
        Task LogStatusChangeAsync(StatusChangeAuditDto auditData);

        Task LogStatusChangeAsync(
            Guid taskId,
            Domain.Entities.TaskStatus oldStatus,
            Domain.Entities.TaskStatus newStatus,
            Guid? userId = null,
            string? reason = null);

        #endregion

        #region Generic Auditing
        Task LogAsync(CreateAuditLogDto auditData);

        Task LogAsync(
            Guid taskId,
            string action,
            string? oldValue = null,
            string? newValue = null,
            Guid? userId = null,
            string? details = null);

        Task LogBatchAsync(IEnumerable<CreateAuditLogDto> auditDataList);

        #endregion

        #region Task Lifecycle Auditing
        Task LogTaskCreatedAsync(Guid taskId, string taskTitle, Guid? userId = null, double? initialPriority = null);

        Task LogTaskUpdatedAsync(Guid taskId, Dictionary<string, (string? oldValue, string? newValue)> changedFields, Guid? userId = null);

        Task LogTaskCompletedAsync(Guid taskId, string taskTitle, Guid? userId = null, TimeSpan? completionTime = null);

        Task LogTaskDeletedAsync(Guid taskId, string taskTitle, Guid? userId = null, string? reason = null);

        #endregion

        #region Query Methods
        Task<AuditLogPagedResponseDto> GetTaskAuditLogsAsync(Guid taskId, int pageNumber = 1, int pageSize = 50);

        Task<AuditLogPagedResponseDto> GetUserAuditLogsAsync(Guid userId, int pageNumber = 1, int pageSize = 50);

        Task<AuditLogPagedResponseDto> GetAuditLogsAsync(AuditLogQueryDto query);

        #endregion

        #region Validation
        bool IsValidAction(string action);

        string NormalizeAction(string action);

        #endregion
    }
}
