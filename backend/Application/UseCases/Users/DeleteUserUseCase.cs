using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Application.Services.Auth;
using EficazAPI.Application.Services.AuditLogs;
using EficazAPI.Application.Services.Shared;

namespace EficazAPI.Application.UseCases.Users
{
    public class DeleteUserUseCase
    {
        private readonly IUserUnitOfWork _userUnitOfWork;

        public DeleteUserUseCase(IUserUnitOfWork userUnitOfWork)
        {
            _userUnitOfWork = userUnitOfWork ?? throw new ArgumentNullException(nameof(userUnitOfWork));
        }

        public async Task<bool> ExecuteAsync(Guid userId)
        {
            await ValidateUserExists(userId);
            await DeleteUser(userId);
            
            return true;
        }

        private async Task ValidateUserExists(Guid userId)
        {
            var user = await _userUnitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException($"Usuário com ID {userId} não foi encontrado no sistema.");
        }

        private async Task DeleteUser(Guid userId)
        {
            await _userUnitOfWork.Users.DeleteAsync(userId);
            await _userUnitOfWork.SaveChangesAsync();
        }
    }
}
