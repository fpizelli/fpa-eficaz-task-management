using EficazAPI.Infrastructure.Repositories.AuditLogs;
using EficazAPI.Application.Ports.AuditLogs;
namespace EficazAPI.Application.Ports.AuditLogs
{
    public interface IAuditUnitOfWork : IDisposable
    {
        IAuditLogRepository AuditLogs { get; }
        Task<int> SaveChangesAsync();
    }
}
