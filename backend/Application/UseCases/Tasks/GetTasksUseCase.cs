using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.Mappers.Tasks;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.UseCases.Shared;

namespace EficazAPI.Application.UseCases.Tasks
{
    public class GetTasksUseCase : IUseCase<List<TaskDto>>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;

        public GetTasksUseCase(ITaskQueryRepository taskQueryRepository)
        {
            _taskQueryRepository = taskQueryRepository ?? throw new ArgumentNullException(nameof(taskQueryRepository));
        }

        public async Task<List<TaskDto>> ExecuteAsync()
        {
            var tasks = await _taskQueryRepository.GetAllAsync();
            return TaskMapper.ToDto(tasks);
        }

        public async Task<TaskDto?> ExecuteAsync(Guid taskId)
        {
            var task = await _taskQueryRepository.GetByIdAsync(taskId);
            return task != null ? TaskMapper.ToDto(task) : null;
        }

        public async Task<List<TaskDto>> GetByUserIdAsync(Guid userId)
        {
            var tasks = await _taskQueryRepository.GetByUserIdAsync(userId);
            return TaskMapper.ToDto(tasks);
        }

        public async Task<List<TaskDto>> GetHighPriorityTasksAsync()
        {
            var tasks = await _taskQueryRepository.GetHighPriorityTasksAsync();
            return TaskMapper.ToDto(tasks);
        }
    }
}
