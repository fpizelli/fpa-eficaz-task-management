namespace EficazAPI.Domain.ValueObjects
{
    public sealed class UserName : IEquatable<UserName>
    {
        private const int MaxLength = 100;
        private const int MinLength = 2;

        public string Value { get; }

        private UserName(string value)
        {
            Value = value;
        }

        public static UserName Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            var trimmedName = name.Trim();

            if (trimmedName.Length < MinLength)
                throw new ArgumentException($"Name must be at least {MinLength} characters", nameof(name));

            if (trimmedName.Length > MaxLength)
                throw new ArgumentException($"Name cannot exceed {MaxLength} characters", nameof(name));

            return new UserName(trimmedName);
        }

        public bool Equals(UserName? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as UserName);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(UserName name)
        {
            return name.Value;
        }
    }
}
