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

namespace EficazAPI.Application.UseCases.Comments
{
    public class GetCommentsByTaskUseCase
    {
        private readonly ICommentUnitOfWork _commentUnitOfWork;

        public GetCommentsByTaskUseCase(ICommentUnitOfWork commentUnitOfWork)
        {
            _commentUnitOfWork = commentUnitOfWork ?? throw new ArgumentNullException(nameof(commentUnitOfWork));
        }

        public async Task<List<CommentDto>> ExecuteAsync(Guid taskId)
        {
            var comments = await _commentUnitOfWork.Comments.GetByTaskIdAsync(taskId);
            return CommentMapper.ToDto(comments);
        }

        public async Task<PagedResult<CommentDto>> ExecutePagedAsync(Guid taskId, PaginationParams pagination)
        {
            var pagedComments = await _commentUnitOfWork.Comments.GetByTaskIdAsync(taskId, pagination);
            var commentDtos = CommentMapper.ToDto(pagedComments.Items);

            return new PagedResult<CommentDto>(
                commentDtos, 
                pagedComments.TotalCount, 
                pagedComments.Page, 
                pagedComments.PageSize);
        }
    }
}
