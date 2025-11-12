using EficazAPI.Infrastructure.Repositories.Users;
using EficazAPI.Application.Ports.Users;
namespace EficazAPI.Application.Ports.Users
{
    public interface IUserUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}
