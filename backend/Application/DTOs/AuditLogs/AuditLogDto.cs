using EficazAPI.Application.DTOs.AuditLogs;
namespace EficazAPI.Application.DTOs.AuditLogs
{
    public class AuditLogDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateAuditLogDto
    {
        public Guid TaskId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public Guid? UserId { get; set; }
        public string? Details { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    public class PriorityChangeAuditDto
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public double OldPriority { get; set; }
        public double NewPriority { get; set; }
        public int? OldImpact { get; set; }
        public int? NewImpact { get; set; }
        public int? OldEffort { get; set; }
        public int? NewEffort { get; set; }
        public int? OldUrgency { get; set; }
        public int? NewUrgency { get; set; }
        public Guid? UserId { get; set; }
        public string ChangeReason { get; set; } = string.Empty;
    }

    public class StatusChangeAuditDto
    {
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public string OldStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
        public string? TransitionReason { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

    public class AuditLogQueryDto
    {
        public Guid? TaskId { get; set; }
        public Guid? UserId { get; set; }
        public List<string> Actions { get; set; } = new();
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true;
    }

    public class AuditLogPagedResponseDto
    {
        public List<AuditLogDto> Logs { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
