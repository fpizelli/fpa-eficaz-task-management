using Microsoft.AspNetCore.Mvc;
using EficazAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EficazAPI.WebApi.Controllers.Shared;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    private readonly DataContext _context;

    public TestController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            Console.WriteLine("[DEBUG] TestController: Buscando usuários diretamente do contexto");
            var users = await _context.Users.ToListAsync();
            Console.WriteLine($"[DEBUG] TestController: Encontrados {users.Count} usuários");
            
            var result = users.Select(u => new {
                u.Id,
                u.Name,
                u.Email,
                Role = u.Role.ToString(),
                RoleNumber = (int)u.Role
            }).ToList();
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] TestController: {ex.Message}");
            return BadRequest(new { error = ex.Message });
        }
    }
}
