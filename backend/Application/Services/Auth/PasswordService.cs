using EficazAPI.Application.Common;
using System.Security.Cryptography;
using System.Text;

namespace EficazAPI.Application.Services.Auth
{
    public class PasswordService : IPasswordService
    {
        private const string DefaultPassword = "senhapadrao";
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Senha não pode ser vazia", nameof(password));

            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;

            try
            {
                var hashBytes = Convert.FromBase64String(hash);
                if (hashBytes.Length != SaltSize + HashSize)
                    return false;

                var salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
                var testHash = pbkdf2.GetBytes(HashSize);

                return CryptographicOperations.FixedTimeEquals(
                    hashBytes.AsSpan(SaltSize, HashSize),
                    testHash
                );
            }
            catch
            {
                return false;
            }
        }

        public bool IsDefaultPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && 
                   password.Equals(DefaultPassword, StringComparison.Ordinal);
        }
    }
}
