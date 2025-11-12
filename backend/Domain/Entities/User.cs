using EficazAPI.Domain.Enums;
using EficazAPI.Domain.ValueObjects;
using EficazAPI.Domain.Events;

namespace EficazAPI.Domain.Entities
{
    public class User
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }

        public User() 
        {
        }

        public User(string name, string email, string passwordHash, Role role = Role.Desenvolvedor)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(passwordHash));

            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            PasswordHash = passwordHash;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string newName)
        {
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
        }

        public void UpdateEmail(Email newEmail)
        {
            Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash cannot be empty", nameof(newPasswordHash));
            
            PasswordHash = newPasswordHash;
        }

        public void ChangeRole(Role newRole)
        {
            Role = newRole;
        }

        public bool HasRole(Role role)
        {
            return Role == role;
        }

        public bool IsAdmin()
        {
            return Role == Role.Admin;
        }

        public bool IsManager()
        {
            return Role == Role.Gerente;
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
