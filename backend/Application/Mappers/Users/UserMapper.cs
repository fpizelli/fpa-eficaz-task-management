using EficazAPI.Application.DTOs.Users;
using EficazAPI.Domain.Entities;

namespace EficazAPI.Application.Mappers.Users
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }

        public static List<UserDto> ToDto(IEnumerable<User> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));

            return users.Select(ToDto).ToList();
        }
    }
}
