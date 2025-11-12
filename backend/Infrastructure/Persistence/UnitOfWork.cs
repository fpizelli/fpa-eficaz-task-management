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
    public class UnitOfWork : ISimpleUnitOfWork
    {
        private readonly DataContext _context;
        private ITaskRepository? _tasks;
        private IUserRepository? _userRepository;
        private ICommentRepository? _comments;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public ICommentRepository Comments => _comments ??= new CommentRepository(_context);

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
