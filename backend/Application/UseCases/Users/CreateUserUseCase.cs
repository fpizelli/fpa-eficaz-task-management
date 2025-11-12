using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.Mappers.Users;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.Services.Auth;
using EficazAPI.Application.UseCases.Shared;
using EficazAPI.Domain.Entities;
using EficazAPI.Domain.ValueObjects;

namespace EficazAPI.Application.UseCases.Users
{
    public class CreateUserUseCase : IUseCase<CreateUserDto, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public CreateUserUseCase(
            IUserRepository userRepository,
            IUserUnitOfWork unitOfWork,
            IPasswordService passwordService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
        }

        public async Task<UserDto> ExecuteAsync(CreateUserDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await ValidateEmailUniqueness(request.Email);

            var name = UserName.Create(request.Name);
            var email = Email.Create(request.Email);
            var passwordHash = _passwordService.HashPassword(request.Password);

            var user = new User(name, email, passwordHash, request.Role);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return UserMapper.ToDto(user);
        }

        private async Task ValidateEmailUniqueness(string email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already in use");
        }
    }
}
