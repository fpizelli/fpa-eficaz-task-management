using EficazAPI.Application.Common;
using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Application.Mappers.Tasks;
using EficazAPI.Application.Mappers.Comments;
using EficazAPI.Application.Mappers.AuditLogs;
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
    public class MoveTaskStatusUseCase
    {
        private readonly ITaskUnitOfWork _taskUnitOfWork;
        private readonly IValidationService _validationService;
        private readonly ISimpleAuditLogService _auditLogService;

        public MoveTaskStatusUseCase(
            ITaskUnitOfWork taskUnitOfWork, 
            IValidationService validationService,
            ISimpleAuditLogService auditLogService)
        {
            _taskUnitOfWork = taskUnitOfWork ?? throw new ArgumentNullException(nameof(taskUnitOfWork));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        }

        public async Task<TaskDto> ExecuteAsync(Guid taskId, UpdateTaskStatusDto statusDto)
        {
            var existingTask = await GetExistingTask(taskId);
            var oldStatus = existingTask.Status;
            
            UpdateTaskStatus(existingTask, statusDto.Status);
            await PersistChanges(existingTask);
            
            await LogStatusChange(existingTask, oldStatus, statusDto.Status);
            
            await LoadUserInformation(existingTask);

            return TaskMapper.ToDto(existingTask);
        }

        private async Task<Domain.Entities.TaskItem> GetExistingTask(Guid taskId)
        {
            await _validationService.ValidateTaskExistsAsync(taskId);
            return _taskUnitOfWork.Tasks.GetById(taskId)!;
        }

        private static void UpdateTaskStatus(Domain.Entities.TaskItem task, Domain.Entities.TaskStatus newStatus)
        {
            task.ChangeStatus(newStatus);
        }

        private async Task PersistChanges(Domain.Entities.TaskItem task)
        {
            _taskUnitOfWork.Tasks.Update(task);
            await _taskUnitOfWork.SaveChangesAsync();
        }

        private async Task LoadUserInformation(Domain.Entities.TaskItem task)
        {
            await Task.CompletedTask;
        }

        private async Task LogStatusChange(Domain.Entities.TaskItem task, Domain.Entities.TaskStatus oldStatus, Domain.Entities.TaskStatus newStatus)
        {
            try
            {
                if (oldStatus != newStatus)
                {
                    await _auditLogService.LogTaskStatusChangedAsync(
                        task.Id,
                        oldStatus.ToString(),
                        newStatus.ToString(),
                        task.UserId
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[WARNING] MoveTaskStatusUseCase: Erro ao registrar auditoria - {ex.Message}");
            }
        }
    }
}
