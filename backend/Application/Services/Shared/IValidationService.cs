namespace EficazAPI.Application.Services.Shared
{
    public interface IValidationService
    {
        Task ValidateTaskExistsAsync(Guid taskId);

        Task ValidateUserExistsAsync(Guid userId);

        void ValidateRequiredText(string? content, string fieldName);
    }
}
