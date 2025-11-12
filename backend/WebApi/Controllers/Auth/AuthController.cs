using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Application.Services.Auth;
using EficazAPI.Application.Services.AuditLogs;
using EficazAPI.Application.Services.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EficazAPI.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Dados inválidos fornecidos"
                    });
                }

                var result = await _authService.LoginAsync(loginDto);

                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AUTH LOGIN ERROR] {ex.Message}");
                Console.WriteLine($"[AUTH LOGIN ERROR] StackTrace: {ex.StackTrace}");
                
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = $"Erro interno do servidor: {ex.Message}"
                });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Dados inválidos fornecidos"
                    });
                }

                var result = await _authService.RegisterAsync(registerDto);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return CreatedAtAction(nameof(Login), result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = "Erro interno do servidor"
                });
            }
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Email é obrigatório");
                }

                var exists = await _authService.EmailExistsAsync(email);
                return Ok(exists);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
