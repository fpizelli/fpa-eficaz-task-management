using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.Mappers.Users;
using EficazAPI.Application.Ports.Users;
using EficazAPI.Application.UseCases.Shared;

namespace EficazAPI.Application.UseCases.Users
{
    public class GetUsersUseCase : IUseCase<List<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<List<UserDto>> ExecuteAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return UserMapper.ToDto(users);
        }

        public async Task<UserDto?> ExecuteAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null ? UserMapper.ToDto(user) : null;
        }
    }
}
