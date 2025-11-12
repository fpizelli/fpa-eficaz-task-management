using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Application.Ports.Tasks;
namespace EficazAPI.Application.Ports.Tasks
{
    public interface ITaskUnitOfWork : IDisposable
    {
        ITaskRepository Tasks { get; }
        Task<int> SaveChangesAsync();
    }
}
