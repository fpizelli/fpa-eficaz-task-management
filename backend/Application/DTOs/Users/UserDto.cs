using EficazAPI.Application.DTOs.Users;
using EficazAPI.Domain.Enums;

namespace EficazAPI.Application.DTOs.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
