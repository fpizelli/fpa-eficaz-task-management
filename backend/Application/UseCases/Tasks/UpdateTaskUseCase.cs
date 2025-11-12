using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.Mappers.Tasks;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.UseCases.Shared;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.ValueObjects;

namespace EficazAPI.Application.UseCases.Tasks
{
    public class UpdateTaskUseCase : IUseCase<UpdateTaskRequest, TaskDto>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskCommandRepository _taskCommandRepository;
        private readonly ITaskUnitOfWork _unitOfWork;

        public UpdateTaskUseCase(
            ITaskQueryRepository taskQueryRepository,
            ITaskCommandRepository taskCommandRepository,
            ITaskUnitOfWork unitOfWork)
        {
            _taskQueryRepository = taskQueryRepository ?? throw new ArgumentNullException(nameof(taskQueryRepository));
            _taskCommandRepository = taskCommandRepository ?? throw new ArgumentNullException(nameof(taskCommandRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<TaskDto> ExecuteAsync(UpdateTaskRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existingTask = await GetExistingTaskAsync(request.TaskId);
            
            UpdateTaskProperties(existingTask, request.UpdateDto);
            
            await _taskCommandRepository.UpdateAsync(existingTask);
            await _unitOfWork.SaveChangesAsync();

            await ProcessDomainEvents(existingTask);

            return TaskMapper.ToDto(existingTask);
        }

        public async Task<TaskDto> ExecuteAsync(Guid taskId, UpdateTaskDto updateDto)
        {
            return await ExecuteAsync(new UpdateTaskRequest { TaskId = taskId, UpdateDto = updateDto });
        }

        private async Task<TaskItem> GetExistingTaskAsync(Guid taskId)
        {
            var task = await _taskQueryRepository.GetByIdAsync(taskId);
            
            if (task == null)
                throw new InvalidOperationException($"Task with ID {taskId} not found.");

            return task;
        }

        private static void UpdateTaskProperties(TaskItem task, UpdateTaskDto updateDto)
        {
            task.UpdateDetails(updateDto.Title, updateDto.Description);
            
            var newPriority = TaskPriority.Create(updateDto.Impact, updateDto.Effort, updateDto.Urgency);
            task.UpdatePriority(newPriority);
        }

        private async Task ProcessDomainEvents(TaskItem task)
        {
            var events = task.GetDomainEvents();
            
            task.ClearDomainEvents();
            
            await Task.CompletedTask;
        }
    }

    public class UpdateTaskRequest
    {
        public Guid TaskId { get; set; }
        public UpdateTaskDto UpdateDto { get; set; } = null!;
    }
}
