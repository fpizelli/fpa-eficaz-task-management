using EficazAPI.Infrastructure.Repositories.Users;
using EficazAPI.Infrastructure.Repositories.Tasks;
using EficazAPI.Application.Common;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;

namespace EficazAPI.Application.Services.Shared
{
    public class ValidationService : IValidationService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public ValidationService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task ValidateTaskExistsAsync(Guid taskId)
        {
            var taskExists = await _taskRepository.GetByIdAsync(taskId);
            
            if (taskExists == null)
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.TaskNotFound, taskId));
        }

        public async Task ValidateUserExistsAsync(Guid userId)
        {
            var userExists = await _userRepository.GetByIdAsync(userId);
            
            if (userExists == null)
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.UserNotFound, userId));
        }

        public void ValidateRequiredText(string? content, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.RequiredFieldEmpty, fieldName));
        }
    }
}
