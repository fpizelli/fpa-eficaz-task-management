namespace EficazAPI.Application.DTOs.Tasks
{
    public class RecalculatePrioritiesResponseDto
    {
        public int ProcessedTasks { get; set; }

        public string Message { get; set; } = string.Empty;

        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
