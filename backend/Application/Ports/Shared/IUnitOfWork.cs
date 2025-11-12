using EficazAPI.Infrastructure.Repositories.AuditLogs;
using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Infrastructure.Repositories.Users;
using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.AuditLogs;
namespace EficazAPI.Application.Ports.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        ICommentRepository Comments { get; }
        IAuditLogRepository AuditLogs { get; }
        Task<int> SaveChangesAsync();
    }
}
