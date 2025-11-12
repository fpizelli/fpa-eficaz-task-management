using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Infrastructure.Repositories.Users;
using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Users;

namespace EficazAPI.Application.Ports.Comments
{
    public interface ICommentUnitOfWork : IDisposable
    {
        ICommentRepository Comments { get; }
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
