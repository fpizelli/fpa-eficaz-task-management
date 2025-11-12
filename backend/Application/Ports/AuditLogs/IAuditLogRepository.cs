using EficazAPI.Infrastructure.Repositories.AuditLogs;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Domain.Entities;
using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;

namespace EficazAPI.Application.Ports.AuditLogs
{
    public interface IAuditLogRepository
    {
        #region Basic CRUD Operations
        Task AddAsync(AuditLog auditLog);

        Task AddBatchAsync(IEnumerable<AuditLog> auditLogs);

        #endregion

        #region Query by Task
        Task<PagedResult<AuditLog>> GetByTaskIdAsync(Guid taskId, int pageNumber = 1, int pageSize = 50);

        Task<List<AuditLog>> GetByTaskIdAsync(Guid taskId);

        Task<List<AuditLog>> GetByTaskIdAndActionsAsync(Guid taskId, IEnumerable<string> actions);

        #endregion

        #region Query by User
        Task<PagedResult<AuditLog>> GetByUserIdAsync(Guid userId, int pageNumber = 1, int pageSize = 50);

        Task<List<AuditLog>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);

        #endregion

        #region Advanced Queries
        Task<AuditLogPagedResponseDto> GetByQueryAsync(AuditLogQueryDto query);

        Task<List<AuditLog>> GetPriorityChangesAsync(Guid? taskId = null, DateTime? startDate = null, DateTime? endDate = null);

        Task<List<AuditLog>> GetStatusChangesAsync(Guid? taskId = null, DateTime? startDate = null, DateTime? endDate = null);

        Task<AuditLog?> GetLastLogByActionAsync(Guid taskId, string action);

        #endregion

        #region Statistics
        Task<int> CountByUserIdAsync(Guid userId);

        Task<int> CountByTaskIdAsync(Guid taskId);

        #endregion
    }
}
