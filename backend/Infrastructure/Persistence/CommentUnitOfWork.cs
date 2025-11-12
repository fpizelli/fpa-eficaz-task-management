using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Infrastructure.Repositories.Users;

namespace EficazAPI.Infrastructure.Persistence
{
    public class CommentUnitOfWork : ICommentUnitOfWork
    {
        private readonly DataContext _context;
        private ICommentRepository? _comments;
        private ITaskRepository? _tasks;
        private IUserRepository? _users;

        public CommentUnitOfWork(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ICommentRepository Comments => _comments ??= new CommentRepository(_context);
        public ITaskRepository Tasks => _tasks ??= new TaskRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);

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
