namespace EficazAPI.Domain.ValueObjects
{
    public sealed class AuditAction : IEquatable<AuditAction>
    {
        private const int MaxLength = 100;

        public string Value { get; }

        private AuditAction(string value)
        {
            Value = value;
        }

        public static AuditAction Create(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
                throw new ArgumentException("Action cannot be empty", nameof(action));

            var trimmedAction = action.Trim();

            if (trimmedAction.Length > MaxLength)
                throw new ArgumentException($"Action cannot exceed {MaxLength} characters", nameof(action));

            return new AuditAction(trimmedAction);
        }

        public static AuditAction TaskCreated => Create("Tarefa criada");
        public static AuditAction TaskUpdated => Create("Tarefa atualizada");
        public static AuditAction TaskDeleted => Create("Tarefa excluída");
        public static AuditAction TaskStatusChanged => Create("Status da tarefa alterado");
        public static AuditAction TaskPriorityChanged => Create("Prioridade da tarefa alterada");
        public static AuditAction TaskAssigned => Create("Tarefa atribuída");
        public static AuditAction TaskUnassigned => Create("Tarefa desatribuída");
        public static AuditAction CommentAdded => Create("Comentário adicionado");
        public static AuditAction CommentUpdated => Create("Comentário atualizado");
        public static AuditAction CommentDeleted => Create("Comentário excluído");

        public bool IsCreation()
        {
            return Value.Contains("criada", StringComparison.OrdinalIgnoreCase) ||
                   Value.Contains("adicionado", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsUpdate()
        {
            return Value.Contains("atualizada", StringComparison.OrdinalIgnoreCase) ||
                   Value.Contains("alterado", StringComparison.OrdinalIgnoreCase) ||
                   Value.Contains("atribuída", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsDeletion()
        {
            return Value.Contains("excluída", StringComparison.OrdinalIgnoreCase) ||
                   Value.Contains("removido", StringComparison.OrdinalIgnoreCase) ||
                   Value.Contains("desatribuída", StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(AuditAction? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AuditAction);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(AuditAction action)
        {
            return action.Value;
        }
    }
}
