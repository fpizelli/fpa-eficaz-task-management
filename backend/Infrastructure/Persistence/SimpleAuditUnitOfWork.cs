using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Infrastructure.Repositories.AuditLogs;

namespace EficazAPI.Infrastructure.Persistence
{
    public class SimpleAuditUnitOfWork : ISimpleAuditUnitOfWork
    {
        private readonly DataContext _context;
        private ISimpleAuditLogRepository? _auditLogRepository;

        public SimpleAuditUnitOfWork(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ISimpleAuditLogRepository AuditLogs => _auditLogRepository ??= new AuditLogRepositorySimple(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
