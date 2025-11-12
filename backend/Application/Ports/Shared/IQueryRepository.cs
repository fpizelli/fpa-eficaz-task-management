using EficazAPI.Application.DTOs.Shared;

namespace EficazAPI.Application.Ports.Shared
{
    public interface IQueryRepository<TEntity, TId> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<List<TEntity>> GetAllAsync();
        Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize);
        Task<bool> ExistsAsync(TId id);
        Task<int> CountAsync();
    }
}
