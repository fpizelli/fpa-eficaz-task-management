using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Ports.AuditLogs
{
    public interface ISimpleAuditLogRepository
    {
        Task<List<AuditLog>> GetAllAsync();
        Task<AuditLog?> GetByIdAsync(Guid id);
        Task<List<AuditLog>> GetByTaskIdAsync(Guid taskId);
        Task AddAsync(AuditLog auditLog);
        Task DeleteAsync(Guid id);
        Task<int> CountAsync();
    }
}
