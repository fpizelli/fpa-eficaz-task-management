using EficazAPI.Application.Ports.AuditLogs;

namespace EficazAPI.Application.Ports.Shared
{
    public interface ISimpleAuditUnitOfWork : IDisposable
    {
        ISimpleAuditLogRepository AuditLogs { get; }
        Task<int> SaveChangesAsync();
    }
}
