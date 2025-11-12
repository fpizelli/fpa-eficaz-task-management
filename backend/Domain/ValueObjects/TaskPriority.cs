namespace EficazAPI.Domain.ValueObjects
{
    public sealed class TaskPriority : IEquatable<TaskPriority>
    {
        private const int MinValue = 1;
        private const int MaxValue = 10;

        public int Impact { get; }
        public int Effort { get; }
        public int Urgency { get; }
        public float Score { get; }

        private TaskPriority(int impact, int effort, int urgency)
        {
            Impact = impact;
            Effort = effort;
            Urgency = urgency;
            Score = CalculateScore(impact, effort, urgency);
        }

        public static TaskPriority Create(int impact, int effort, int urgency)
        {
            ValidateValue(impact, nameof(impact));
            ValidateValue(effort, nameof(effort));
            ValidateValue(urgency, nameof(urgency));

            return new TaskPriority(impact, effort, urgency);
        }

        private static void ValidateValue(int value, string paramName)
        {
            if (value < MinValue || value > MaxValue)
                throw new ArgumentOutOfRangeException(paramName, 
                    $"Value must be between {MinValue} and {MaxValue}");
        }

        private static float CalculateScore(int impact, int effort, int urgency)
        {
            return (float)(impact * urgency) / effort;
        }

        public TaskPriority UpdateImpact(int newImpact)
        {
            return Create(newImpact, Effort, Urgency);
        }

        public TaskPriority UpdateEffort(int newEffort)
        {
            return Create(Impact, newEffort, Urgency);
        }

        public TaskPriority UpdateUrgency(int newUrgency)
        {
            return Create(Impact, Effort, newUrgency);
        }

        public bool Equals(TaskPriority? other)
        {
            return other is not null && 
                   Impact == other.Impact && 
                   Effort == other.Effort && 
                   Urgency == other.Urgency;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TaskPriority);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Impact, Effort, Urgency);
        }

        public override string ToString()
        {
            return $"Priority(I:{Impact}, E:{Effort}, U:{Urgency}, Score:{Score:F2})";
        }
    }
}
