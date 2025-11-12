using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Mappers.AuditLogs
{
    public static class AuditLogMapper
    {
        public static AuditLogDto ToDto(AuditLog auditLog)
        {
            if (auditLog == null)
                throw new ArgumentNullException(nameof(auditLog));

            return new AuditLogDto
            {
                Id = auditLog.Id,
                TaskId = auditLog.TaskId,
                Action = auditLog.Action,
                OldValue = auditLog.OldValue,
                NewValue = auditLog.NewValue,
                UserId = auditLog.UserId,
                UserName = auditLog.User?.Name,
                CreatedAt = auditLog.CreatedAt
            };
        }

        public static List<AuditLogDto> ToDto(IEnumerable<AuditLog> auditLogs)
        {
            if (auditLogs == null)
                throw new ArgumentNullException(nameof(auditLogs));

            return auditLogs.Select(ToDto).ToList();
        }
    }
}
