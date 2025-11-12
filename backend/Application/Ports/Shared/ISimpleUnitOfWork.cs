using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.Comments;

namespace EficazAPI.Application.Ports.Shared
{
    public interface ISimpleUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        ICommentRepository Comments { get; }
        Task<int> SaveChangesAsync();
    }
}
