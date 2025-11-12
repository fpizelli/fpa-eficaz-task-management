namespace EficazAPI.Application.Services.Auth
{
    public interface IPasswordService
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string hash);

        bool IsDefaultPassword(string password);
    }
}
