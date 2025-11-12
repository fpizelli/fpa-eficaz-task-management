namespace EficazAPI.Domain.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        private const int MaxLength = 255;
        private static readonly System.Text.RegularExpressions.Regex EmailRegex = 
            new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
                System.Text.RegularExpressions.RegexOptions.Compiled);

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (email.Length > MaxLength)
                throw new ArgumentException($"Email cannot exceed {MaxLength} characters", nameof(email));

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format", nameof(email));

            return new Email(email.ToLowerInvariant());
        }

        public bool Equals(Email? other)
        {
            return other is not null && Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Email);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }
    }
}
