using EficazAPI.Domain.Entities;
using System.Linq.Expressions;
using TaskStatus = EficazAPI.Domain.Entities.TaskStatus;

namespace EficazAPI.Domain.Specifications
{
    public static class TaskSpecifications
    {
        public static ISpecification<TaskItem> HighPriority(float minScore = 8.0f)
        {
            return new HighPrioritySpecification(minScore);
        }

        public static ISpecification<TaskItem> AssignedToUser(Guid userId)
        {
            return new AssignedToUserSpecification(userId);
        }

        public static ISpecification<TaskItem> WithStatus(TaskStatus status)
        {
            return new WithStatusSpecification(status);
        }

        public static ISpecification<TaskItem> Overdue(int daysOld = 7)
        {
            return new OverdueSpecification(daysOld);
        }

        public static ISpecification<TaskItem> CreatedAfter(DateTime date)
        {
            return new CreatedAfterSpecification(date);
        }
    }

    internal class HighPrioritySpecification : ISpecification<TaskItem>
    {
        private readonly float _minScore;

        public HighPrioritySpecification(float minScore)
        {
            _minScore = minScore;
        }

        public Expression<Func<TaskItem, bool>> ToExpression()
        {
            return task => task.Priority.Score >= _minScore;
        }

        public bool IsSatisfiedBy(TaskItem entity)
        {
            return entity.Priority.Score >= _minScore;
        }
    }

    internal class AssignedToUserSpecification : ISpecification<TaskItem>
    {
        private readonly Guid _userId;

        public AssignedToUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public Expression<Func<TaskItem, bool>> ToExpression()
        {
            return task => task.UserId == _userId;
        }

        public bool IsSatisfiedBy(TaskItem entity)
        {
            return entity.UserId == _userId;
        }
    }

    internal class WithStatusSpecification : ISpecification<TaskItem>
    {
        private readonly TaskStatus _status;

        public WithStatusSpecification(TaskStatus status)
        {
            _status = status;
        }

        public Expression<Func<TaskItem, bool>> ToExpression()
        {
            return task => task.Status == _status;
        }

        public bool IsSatisfiedBy(TaskItem entity)
        {
            return entity.Status == _status;
        }
    }

    internal class OverdueSpecification : ISpecification<TaskItem>
    {
        private readonly int _daysOld;

        public OverdueSpecification(int daysOld)
        {
            _daysOld = daysOld;
        }

        public Expression<Func<TaskItem, bool>> ToExpression()
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysOld);
            return task => task.CreatedAt <= cutoffDate && task.Status != TaskStatus.Done;
        }

        public bool IsSatisfiedBy(TaskItem entity)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-_daysOld);
            return entity.CreatedAt <= cutoffDate && entity.Status != TaskStatus.Done;
        }
    }

    internal class CreatedAfterSpecification : ISpecification<TaskItem>
    {
        private readonly DateTime _date;

        public CreatedAfterSpecification(DateTime date)
        {
            _date = date;
        }

        public Expression<Func<TaskItem, bool>> ToExpression()
        {
            return task => task.CreatedAt >= _date;
        }

        public bool IsSatisfiedBy(TaskItem entity)
        {
            return entity.CreatedAt >= _date;
        }
    }
}
