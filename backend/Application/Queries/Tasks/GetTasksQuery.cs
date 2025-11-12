using EficazAPI.Application.DTOs.Tasks;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Application.Queries.Tasks
{
    /// <summary>
    /// Query para busca de tarefas - aplica CQRS
    /// Aplica Command Query Responsibility Segregation
    /// </summary>
    public class GetTasksQuery : IQuery<List<TaskDto>>
    {
        public Guid? UserId { get; set; }
        public TaskStatus? Status { get; set; }
        public bool? HighPriorityOnly { get; set; }
        public int? DaysOld { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
