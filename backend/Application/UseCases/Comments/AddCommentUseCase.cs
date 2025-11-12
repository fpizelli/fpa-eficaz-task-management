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
using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.UseCases.Comments
{
    public class AddCommentUseCase
    {
        private readonly ICommentUnitOfWork _commentUnitOfWork;
        private readonly IValidationService _validationService;

        public AddCommentUseCase(ICommentUnitOfWork commentUnitOfWork, IValidationService validationService)
        {
            _commentUnitOfWork = commentUnitOfWork ?? throw new ArgumentNullException(nameof(commentUnitOfWork));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
        }

        public async Task<CommentDto> ExecuteAsync(CreateCommentDto createCommentDto)
        {
            await ValidateCommentCreation(createCommentDto);
            
            var newComment = CreateCommentEntity(createCommentDto);
            await PersistComment(newComment);
            await LoadUserInformation(newComment);

            return CommentMapper.ToDto(newComment);
        }

        private async Task ValidateCommentCreation(CreateCommentDto dto)
        {
            _validationService.ValidateRequiredText(dto.Content, Constants.FieldNames.CommentContent);
            await _validationService.ValidateTaskExistsAsync(dto.TaskId);
            await _validationService.ValidateUserExistsAsync(dto.UserId);
        }

        private static Comment CreateCommentEntity(CreateCommentDto dto)
        {
            return new Comment(dto.TaskId, dto.UserId, dto.Content);
        }

        private async Task PersistComment(Comment comment)
        {
            await _commentUnitOfWork.Comments.AddAsync(comment);
            await _commentUnitOfWork.SaveChangesAsync();
        }

        private async Task LoadUserInformation(Comment comment)
        {
            comment.User = await _commentUnitOfWork.Users.GetByIdAsync(comment.UserId);
        }

    }
}
