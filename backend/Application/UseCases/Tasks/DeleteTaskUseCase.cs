using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Application.Services.Auth;
using EficazAPI.Application.Services.AuditLogs;
using EficazAPI.Application.Services.Shared;

namespace EficazAPI.Application.UseCases.Tasks
{
    public class DeleteTaskUseCase
    {
        private readonly ITaskUnitOfWork _taskUnitOfWork;
        private readonly IValidationService _validationService;

        public DeleteTaskUseCase(ITaskUnitOfWork taskUnitOfWork, IValidationService validationService)
        {
            _taskUnitOfWork = taskUnitOfWork ?? throw new ArgumentNullException(nameof(taskUnitOfWork));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        public async Task<bool> ExecuteAsync(Guid taskId)
        {
            await ValidateTaskExists(taskId);
            DeleteTask(taskId);
            await PersistChanges();
            
            return true;
        }

        private async Task ValidateTaskExists(Guid taskId)
        {
            await _validationService.ValidateTaskExistsAsync(taskId);
        }

        private void DeleteTask(Guid taskId)
        {
            _taskUnitOfWork.Tasks.Delete(taskId);
        }

        private async Task PersistChanges()
        {
            await _taskUnitOfWork.SaveChangesAsync();
        }
    }
}
