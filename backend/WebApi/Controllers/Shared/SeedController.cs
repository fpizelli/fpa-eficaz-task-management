using EficazAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EficazAPI.WebApi.Controllers.Shared
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class SeedController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;

        public SeedController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost("run")]
        public async Task<IActionResult> RunSeed()
        {
            try
            {
                if (!_environment.IsDevelopment())
                {
                    return BadRequest(new { 
                        error = "Seed só pode ser executado em ambiente de desenvolvimento",
                        environment = _environment.EnvironmentName 
                    });
                }

                await SimpleDbSeeder.SeedData(_context);

                int commentCount = 0;
                int auditCount = 0;
                try
                {
                    commentCount = await _context.Comments.CountAsync();
                    auditCount = await _context.AuditLogs.CountAsync();
                }
                catch
                {
                }

                var stats = new
                {
                    users = await _context.Users.CountAsync(),
                    tasks = await _context.Tasks.CountAsync(),
                    comments = commentCount,
                    auditLogs = auditCount,
                    timestamp = DateTime.UtcNow,
                    message = "Seed executado com sucesso!"
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "Erro ao executar seed", 
                    details = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var stats = new
                {
                    users = await _context.Users.CountAsync(),
                    tasks = await _context.Tasks.CountAsync(),
                    comments = await _context.Comments.CountAsync(),
                    auditLogs = await _context.AuditLogs.CountAsync(),
                    environment = _environment.EnvironmentName,
                    timestamp = DateTime.UtcNow
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "Erro ao obter estatísticas", 
                    details = ex.Message 
                });
            }
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearData()
        {
            try
            {
                if (!_environment.IsDevelopment())
                {
                    return BadRequest(new { 
                        error = "Limpeza só pode ser executada em ambiente de desenvolvimento",
                        environment = _environment.EnvironmentName 
                    });
                }

                _context.AuditLogs.RemoveRange(_context.AuditLogs);
                _context.Comments.RemoveRange(_context.Comments);
                _context.Tasks.RemoveRange(_context.Tasks);
                _context.Users.RemoveRange(_context.Users);
                
                await _context.SaveChangesAsync();

                return Ok(new { 
                    message = "Todos os dados foram removidos com sucesso!",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    error = "Erro ao limpar dados", 
                    details = ex.Message 
                });
            }
        }
    }
}
