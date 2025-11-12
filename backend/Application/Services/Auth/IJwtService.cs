using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Services.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);

        bool ValidateToken(string token);

        Guid? GetUserIdFromToken(string token);

        DateTime GetTokenExpiration();
    }
}
