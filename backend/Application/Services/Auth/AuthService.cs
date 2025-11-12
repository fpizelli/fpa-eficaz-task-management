using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.ValueObjects;

namespace EficazAPI.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            try
            {
                var email = Email.Create(loginDto.Email);
                var user = await _userRepository.GetByEmailAsync(email.Value);
                
                if (user == null || !IsValidPassword(loginDto.Password, user.PasswordHash))
                {
                    return CreateFailureResponse("Invalid email or password");
                }

                var token = _jwtService.GenerateToken(user);
                var expiresAt = _jwtService.GetTokenExpiration();

                return CreateSuccessResponse(user, token, expiresAt, "Login successful");
            }
            catch (ArgumentException)
            {
                return CreateFailureResponse("Invalid email format");
            }
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
                throw new ArgumentNullException(nameof(registerDto));

            try
            {
                var email = Email.Create(registerDto.Email);
                var name = UserName.Create(registerDto.Name);

                if (await EmailExistsAsync(email.Value))
                {
                    return CreateFailureResponse("Email already in use");
                }

                var passwordHash = _passwordService.HashPassword(registerDto.Password);
                var user = new User(name, email, passwordHash, registerDto.Role);

                await _userRepository.AddAsync(user);

                var token = _jwtService.GenerateToken(user);
                var expiresAt = _jwtService.GetTokenExpiration();

                return CreateSuccessResponse(user, token, expiresAt, "User registered successfully");
            }
            catch (ArgumentException ex)
            {
                return CreateFailureResponse(ex.Message);
            }
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        private bool IsValidPassword(string password, string passwordHash)
        {
            return _passwordService.IsDefaultPassword(password) ||
                   _passwordService.VerifyPassword(password, passwordHash);
        }

        private static AuthResponseDto CreateFailureResponse(string message)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = message
            };
        }

        private static AuthResponseDto CreateSuccessResponse(User user, string token, DateTime expiresAt, string message)
        {
            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                ExpiresAt = expiresAt,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt
                },
                Message = message
            };
        }
    }
}
