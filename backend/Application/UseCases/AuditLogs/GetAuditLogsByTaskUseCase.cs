using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Application.Mappers.Tasks;
using EficazAPI.Application.Mappers.Comments;
using EficazAPI.Application.Mappers.AuditLogs;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;

namespace EficazAPI.Application.UseCases.AuditLogs
{
    public class GetAuditLogsByTaskUseCase
    {
        private readonly ISimpleAuditUnitOfWork _auditUnitOfWork;

        public GetAuditLogsByTaskUseCase(ISimpleAuditUnitOfWork auditUnitOfWork)
        {
            _auditUnitOfWork = auditUnitOfWork ?? throw new ArgumentNullException(nameof(auditUnitOfWork));
        }

        public async Task<List<AuditLogDto>> ExecuteAsync(Guid taskId)
        {
            var auditLogs = await _auditUnitOfWork.AuditLogs.GetByTaskIdAsync(taskId);
            return AuditLogMapper.ToDto(auditLogs);
        }

        public async Task<List<AuditLogDto>> ExecutePagedAsync(Guid taskId, int pageNumber = 1, int pageSize = 50)
        {
            var auditLogs = await _auditUnitOfWork.AuditLogs.GetByTaskIdAsync(taskId);
            var pagedLogs = auditLogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return AuditLogMapper.ToDto(pagedLogs);
        }
    }
}
