using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Domain.Entities;
using EficazAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EficazAPI.Infrastructure.Repositories.AuditLogs
{
    public class AuditLogRepositorySimple : ISimpleAuditLogRepository
    {
        private readonly DataContext _context;

        public AuditLogRepositorySimple(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .Include(a => a.Task)
                .Include(a => a.User)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<AuditLog?> GetByIdAsync(Guid id)
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .Include(a => a.Task)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<AuditLog>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .Include(a => a.User)
                .Where(a => a.TaskId == taskId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var auditLog = await _context.AuditLogs.FindAsync(id);
            if (auditLog != null)
            {
                _context.AuditLogs.Remove(auditLog);
            }
        }

        public async Task<int> CountAsync()
        {
            return await _context.AuditLogs.CountAsync();
        }
    }
}
