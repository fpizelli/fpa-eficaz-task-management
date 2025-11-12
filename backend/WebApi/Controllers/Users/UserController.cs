using Microsoft.AspNetCore.Mvc;
using EficazAPI.Application.UseCases.Users;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Domain.Enums;
using EficazAPI.WebApi.Attributes;
using EficazAPI.WebApi.Controllers.Shared;

namespace EficazAPI.WebApi.Controllers.Users
{
    /// <summary>
    /// Controller para operações de usuários - aplica SRP
    /// Aplica Single Responsibility Principle - apenas coordenação HTTP
    /// </summary>
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly GetUsersUseCase _getUsers;
        private readonly CreateUserUseCase _createUser;
        private readonly DeleteUserUseCase _deleteUser;

        public UserController(
            GetUsersUseCase getUsers,
            CreateUserUseCase createUser,
            DeleteUserUseCase deleteUser)
        {
            _getUsers = getUsers ?? throw new ArgumentNullException(nameof(getUsers));
            _createUser = createUser ?? throw new ArgumentNullException(nameof(createUser));
            _deleteUser = deleteUser ?? throw new ArgumentNullException(nameof(deleteUser));
        }

        [HttpGet("test")]
        public ActionResult Test()
        {
            return HandleSuccess(new { message = "UserController working", timestamp = DateTime.UtcNow });
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var users = await _getUsers.ExecuteAsync();
                return HandleSuccess(users);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _getUsers.ExecuteAsync(id);
                if (user == null) 
                    return NotFound(new { message = $"User with ID {id} not found." });
                
                return HandleSuccess(user);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        // [RequireRole(Role.Gerente)] // Temporarily removed for testing
        public async Task<IActionResult> Create([FromBody] CreateUserDto userDto)
        {
            try
            {
                var createdUser = await _createUser.ExecuteAsync(userDto);
                return HandleCreated(createdUser, nameof(GetById), new { id = createdUser.Id });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id:guid}")]
        [RequireRole(Role.Gerente)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _deleteUser.ExecuteAsync(id);
                return HandleNoContent("User deleted successfully");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
