using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Mappers.Comments
{
    public static class CommentMapper
    {
        private const string UnknownUserName = "Usuário desconhecido";

        public static CommentDto ToDto(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            return new CommentDto
            {
                Id = comment.Id,
                TaskId = comment.TaskId,
                UserId = comment.UserId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserName = comment.User?.Name ?? UnknownUserName
            };
        }

        public static List<CommentDto> ToDto(IEnumerable<Comment> comments)
        {
            if (comments == null)
                throw new ArgumentNullException(nameof(comments));

            return comments.Select(ToDto).ToList();
        }
    }
}
