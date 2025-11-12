using EficazAPI.Infrastructure.Repositories.Comments;
using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.Ports.Comments;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Ports.AuditLogs;
using EficazAPI.Application.Ports.Shared;
using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Domain.Entities;
using EficazAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EficazAPI.Infrastructure.Repositories.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;

        public CommentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Comment>> GetByTaskIdAsync(Guid taskId, PaginationParams pagination)
        {
            var query = _context.Comments
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId)
                .OrderBy(c => c.CreatedAt);

            var totalCount = await query.CountAsync();
            
            var items = await query
                .Skip(pagination.Skip)
                .Take(pagination.Take)
                .ToListAsync();

            return new PagedResult<Comment>(items, totalCount, pagination.Page, pagination.PageSize);
        }

        public async Task<List<Comment>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.TaskId == taskId)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
        }
    }
}
