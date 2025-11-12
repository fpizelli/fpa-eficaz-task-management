using EficazAPI.Domain.Enums;

namespace EficazAPI.Application.DTOs.Users
{
    public class CreateUserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.Desenvolvedor;
    }
}
