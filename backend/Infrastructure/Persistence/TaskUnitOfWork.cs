using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Infrastructure.Repositories.Users;
using EficazAPI.Infrastructure.Repositories.AuditLogs;

namespace EficazAPI.Infrastructure.Persistence
{
    public class TaskUnitOfWork : ITaskUnitOfWork
    {
        private readonly DataContext _context;
        private ITaskRepository? _tasks;

        public TaskUnitOfWork(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);

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
