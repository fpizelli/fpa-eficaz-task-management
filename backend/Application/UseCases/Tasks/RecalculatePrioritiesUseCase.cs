using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.UseCases.Shared;
using EficazAPI.Domain.Services;

namespace EficazAPI.Application.UseCases.Tasks
{
    public class RecalculatePrioritiesUseCase : IUseCase<RecalculatePrioritiesResponseDto>
    {
        private readonly ITaskQueryRepository _taskQueryRepository;
        private readonly ITaskCommandRepository _taskCommandRepository;
        private readonly ITaskUnitOfWork _unitOfWork;
        private readonly ITaskDomainService _taskDomainService;

        public RecalculatePrioritiesUseCase(
            ITaskQueryRepository taskQueryRepository,
            ITaskCommandRepository taskCommandRepository,
            ITaskUnitOfWork unitOfWork,
            ITaskDomainService taskDomainService)
        {
            _taskQueryRepository = taskQueryRepository ?? throw new ArgumentNullException(nameof(taskQueryRepository));
            _taskCommandRepository = taskCommandRepository ?? throw new ArgumentNullException(nameof(taskCommandRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _taskDomainService = taskDomainService ?? throw new ArgumentNullException(nameof(taskDomainService));
        }

        public async Task<RecalculatePrioritiesResponseDto> ExecuteAsync()
        {
            var tasks = await _taskQueryRepository.GetAllAsync();
            
            var recalculatedTasks = await _taskDomainService.RecalculateAllPrioritiesAsync(tasks);

            foreach (var task in recalculatedTasks)
            {
                await _taskCommandRepository.UpdateAsync(task);
            }
            
            await _unitOfWork.SaveChangesAsync();

            return new RecalculatePrioritiesResponseDto
            {
                ProcessedTasks = recalculatedTasks.Count,
                Message = $"Successfully recalculated {recalculatedTasks.Count} tasks",
                ExecutedAt = DateTime.UtcNow
            };
        }
    }
}
