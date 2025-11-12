using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.Mappers.Tasks;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Application.UseCases.Shared;
using EficazAPI.Application.Services.AuditLogs;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.ValueObjects;
using EficazAPI.Domain.Events;

namespace EficazAPI.Application.UseCases.Tasks
{
    public class CreateTaskUseCase : IUseCase<CreateTaskDto, TaskDto>
    {
        private readonly ITaskCommandRepository _taskCommandRepository;
        private readonly ITaskUnitOfWork _unitOfWork;
        private readonly ISimpleAuditLogService _auditLogService;

        public CreateTaskUseCase(
            ITaskCommandRepository taskCommandRepository,
            ITaskUnitOfWork unitOfWork,
            ISimpleAuditLogService auditLogService)
        {
            _taskCommandRepository = taskCommandRepository ?? throw new ArgumentNullException(nameof(taskCommandRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        }

        public async Task<TaskDto> ExecuteAsync(CreateTaskDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var priority = TaskPriority.Create(request.Impact, request.Effort, request.Urgency);
            
            var userId = ParseUserId(request.UserId);
            var task = new TaskItem(request.Title, request.Description, priority, userId);

            await _taskCommandRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogTaskCreatedAsync(task.Id, task.Title, task.UserId, task.Priority.Score);

            await ProcessDomainEvents(task);

            return TaskMapper.ToDto(task);
        }

        private static Guid? ParseUserId(string? userIdString)
        {
            if (string.IsNullOrWhiteSpace(userIdString))
                return null;

            return Guid.TryParse(userIdString, out var userId) ? userId : null;
        }

        private async Task ProcessDomainEvents(TaskItem task)
        {
            var events = task.GetDomainEvents();
            
            task.ClearDomainEvents();
            
            await Task.CompletedTask;
        }
    }
}
