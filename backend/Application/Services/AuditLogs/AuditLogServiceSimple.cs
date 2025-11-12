using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.ValueObjects;

namespace EficazAPI.Application.Services.AuditLogs
{
    public class AuditLogServiceSimple : ISimpleAuditLogService
    {
        private readonly ISimpleAuditLogRepository _auditLogRepository;

        public AuditLogServiceSimple(ISimpleAuditLogRepository auditLogRepository)
        {
            _auditLogRepository = auditLogRepository ?? throw new ArgumentNullException(nameof(auditLogRepository));
        }

        public async Task LogTaskCreatedAsync(Guid taskId, string title, Guid? userId, float priorityScore)
        {
            var auditLog = new AuditLog(
                taskId,
                AuditAction.TaskCreated,
                null,
                $"Task '{title}' created with priority score {priorityScore:F2}",
                userId
            );

            await _auditLogRepository.AddAsync(auditLog);
        }

        public async Task LogTaskUpdatedAsync(Guid taskId, string changes, Guid? userId)
        {
            var auditLog = new AuditLog(
                taskId,
                AuditAction.TaskUpdated,
                null,
                changes,
                userId
            );

            await _auditLogRepository.AddAsync(auditLog);
        }

        public async Task LogTaskDeletedAsync(Guid taskId, string title, Guid? userId)
        {
            var auditLog = new AuditLog(
                taskId,
                AuditAction.TaskDeleted,
                title,
                null,
                userId
            );

            await _auditLogRepository.AddAsync(auditLog);
        }

        public async Task LogTaskStatusChangedAsync(Guid taskId, string oldStatus, string newStatus, Guid? userId)
        {
            var auditLog = new AuditLog(
                taskId,
                AuditAction.TaskStatusChanged,
                oldStatus,
                newStatus,
                userId
            );

            await _auditLogRepository.AddAsync(auditLog);
        }
    }
}
