using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Domain.Entities;
using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;

namespace EficazAPI.Application.Ports.Comments
{
    public interface ICommentRepository
    {
        Task<PagedResult<Comment>> GetByTaskIdAsync(Guid taskId, PaginationParams pagination);
        Task<List<Comment>> GetByTaskIdAsync(Guid taskId);
        Task<Comment?> GetByIdAsync(Guid id);
        Task AddAsync(Comment comment);
        Task DeleteAsync(Guid id);
    }
}
