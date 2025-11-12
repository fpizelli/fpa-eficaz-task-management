using EficazAPI.Application.DTOs.Shared;
namespace EficazAPI.Application.DTOs.Shared
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

        public PagedResult()
        {
        }

        public PagedResult(List<T> items, int totalCount, int page, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }

    public class PaginationParams
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int Skip => (Page - 1) * PageSize;
        public int Take => PageSize;

        public PaginationParams()
        {
        }

        public PaginationParams(int page, int pageSize)
        {
            Page = Math.Max(1, page);
            PageSize = Math.Max(1, Math.Min(100, pageSize));
        }
    }
}
