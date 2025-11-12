using EficazAPI.Application.Ports.Tasks;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Infrastructure.Persistence;
using EficazAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Infrastructure.Repositories.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext _context;

        public TaskRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .OrderByDescending(t => t.Priority.Score)
                .ToListAsync();
        }

        public async Task<PagedResult<TaskItem>> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .OrderByDescending(t => t.Priority.Score);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<TaskItem>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Tasks.CountAsync();
        }

        public async Task<List<TaskItem>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Priority.Score)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.Priority.Score)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetHighPriorityTasksAsync(float minPriorityScore = 8.0f)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.Priority.Score >= minPriorityScore)
                .OrderByDescending(t => t.Priority.Score)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetOverdueTasksAsync(int daysOld = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.CreatedAt <= cutoffDate && t.Status != TaskStatus.Done)
                .OrderByDescending(t => t.Priority.Score)
                .ToListAsync();
        }

        public void Add(TaskItem entity)
        {
            _context.Tasks.Add(entity);
        }

        public void Update(TaskItem entity)
        {
            _context.Tasks.Update(entity);
        }

        public void Delete(Guid id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }
        }

        public void Delete(TaskItem entity)
        {
            _context.Tasks.Remove(entity);
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            var entry = await _context.Tasks.AddAsync(task);
            return entry.Entity;
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await Task.CompletedTask;
        }

        public List<TaskItem> GetAll()
        {
            return _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .OrderByDescending(t => t.Priority.Score)
                .ToList();
        }

        public TaskItem? GetById(Guid id)
        {
            return _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<TaskItem> GetByUserId(Guid userId)
        {
            return _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Priority.Score)
                .ToList();
        }

        public List<TaskItem> GetByStatus(TaskStatus status)
        {
            return _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.Priority.Score)
                .ToList();
        }
    }
}
