using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EficazAPI.Application.Services.Auth;
using EficazAPI.Application.Services.AuditLogs;
using EficazAPI.Application.Services.Shared;
using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;

namespace EficazAPI.WebApi.Controllers.AuditLogs;

[ApiController]
[Route("api/audit")]
public class AuditController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
    }

    #region Task Audit Endpoints

    [HttpGet("task/{taskId:guid}")]
    [ProducesResponseType(typeof(AuditLogPagedResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuditLogPagedResponseDto>> GetTaskAuditLogs(
        Guid taskId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            Console.WriteLine($"[DEBUG] AuditController.GetTaskAuditLogs: Tarefa {taskId}, Página {pageNumber}");

            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new { error = "Parâmetros de paginação inválidos" });
            }

            var auditLogs = await _auditLogService.GetTaskAuditLogsAsync(taskId, pageNumber, pageSize);
            
            Console.WriteLine($"[DEBUG] AuditController.GetTaskAuditLogs: {auditLogs.Logs.Count} logs encontrados");
            
            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.GetTaskAuditLogs: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno ao buscar histórico", details = ex.Message });
        }
    }

    [HttpGet("task/{taskId:guid}/priority-changes")]
    [ProducesResponseType(typeof(List<AuditLogDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<AuditLogDto>>> GetTaskPriorityChanges(Guid taskId)
    {
        try
        {
            var query = new AuditLogQueryDto
            {
                TaskId = taskId,
                Actions = new List<string> { "Priority Changed", "Impact Changed", "Effort Changed", "Urgency Changed" },
                PageSize = 100,
                SortBy = "CreatedAt",
                SortDescending = true
            };

            var result = await _auditLogService.GetAuditLogsAsync(query);
            return Ok(result.Logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.GetTaskPriorityChanges: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno ao buscar mudanças de prioridade", details = ex.Message });
        }
    }

    [HttpGet("task/{taskId:guid}/status-changes")]
    [ProducesResponseType(typeof(List<AuditLogDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<AuditLogDto>>> GetTaskStatusChanges(Guid taskId)
    {
        try
        {
            var query = new AuditLogQueryDto
            {
                TaskId = taskId,
                Actions = new List<string> { "Status Changed", "Task Started", "Task Completed", "Task Reopened" },
                PageSize = 100,
                SortBy = "CreatedAt",
                SortDescending = true
            };

            var result = await _auditLogService.GetAuditLogsAsync(query);
            return Ok(result.Logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.GetTaskStatusChanges: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno ao buscar mudanças de status", details = ex.Message });
        }
    }

    #endregion

    #region User Audit Endpoints

    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(AuditLogPagedResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuditLogPagedResponseDto>> GetUserAuditLogs(
        Guid userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            Console.WriteLine($"[DEBUG] AuditController.GetUserAuditLogs: Usuário {userId}, Página {pageNumber}");

            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new { error = "Parâmetros de paginação inválidos" });
            }

            var auditLogs = await _auditLogService.GetUserAuditLogsAsync(userId, pageNumber, pageSize);
            
            Console.WriteLine($"[DEBUG] AuditController.GetUserAuditLogs: {auditLogs.Logs.Count} logs encontrados");
            
            return Ok(auditLogs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.GetUserAuditLogs: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno ao buscar histórico do usuário", details = ex.Message });
        }
    }

    [HttpGet("user/{userId:guid}/activity")]
    [ProducesResponseType(typeof(List<AuditLogDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<AuditLogDto>>> GetUserActivity(
        Guid userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var query = new AuditLogQueryDto
            {
                UserId = userId,
                StartDate = startDate ?? DateTime.UtcNow.AddDays(-30),
                EndDate = endDate ?? DateTime.UtcNow,
                PageSize = 100,
                SortBy = "CreatedAt",
                SortDescending = true
            };

            var result = await _auditLogService.GetAuditLogsAsync(query);
            return Ok(result.Logs);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.GetUserActivity: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno ao buscar atividade do usuário", details = ex.Message });
        }
    }

    #endregion

    #region Advanced Query Endpoints

    [HttpPost("search")]
    [ProducesResponseType(typeof(AuditLogPagedResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuditLogPagedResponseDto>> SearchAuditLogs([FromBody] AuditLogQueryDto query)
    {
        try
        {
            Console.WriteLine($"[DEBUG] AuditController.SearchAuditLogs: Consulta avançada");

            if (query == null)
            {
                return BadRequest(new { error = "Query não pode ser nula" });
            }

            if (query.PageNumber < 1 || query.PageSize < 1 || query.PageSize > 100)
            {
                return BadRequest(new { error = "Parâmetros de paginação inválidos" });
            }

            if (query.StartDate.HasValue && query.EndDate.HasValue && query.StartDate > query.EndDate)
            {
                return BadRequest(new { error = "Data inicial deve ser anterior à data final" });
            }

            var result = await _auditLogService.GetAuditLogsAsync(query);
            
            Console.WriteLine($"[DEBUG] AuditController.SearchAuditLogs: {result.Logs.Count} logs encontrados");
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.SearchAuditLogs: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno na consulta avançada", details = ex.Message });
        }
    }

    [HttpGet("period")]
    [ProducesResponseType(typeof(AuditLogPagedResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<AuditLogPagedResponseDto>> GetAuditLogsByPeriod(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] string[]? actions = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            if (startDate >= endDate)
            {
                return BadRequest(new { error = "Data inicial deve ser anterior à data final" });
            }

            if ((endDate - startDate).TotalDays > 90)
            {
                return BadRequest(new { error = "Período não pode ser superior a 90 dias" });
            }

            var query = new AuditLogQueryDto
            {
                StartDate = startDate,
                EndDate = endDate,
                Actions = actions?.ToList() ?? new List<string>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortBy = "CreatedAt",
                SortDescending = true
            };

            var result = await _auditLogService.GetAuditLogsAsync(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] AuditController.GetAuditLogsByPeriod: {ex.Message}");
            return StatusCode(500, new { error = "Erro interno ao buscar logs por período", details = ex.Message });
        }
    }

    #endregion

    #region Utility Endpoints

    [HttpGet("actions")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesResponseType(401)]
    public IActionResult GetAvailableActions()
    {
        var actions = new List<string>
        {
            "Priority Changed",
            "Impact Changed", 
            "Effort Changed",
            "Urgency Changed",
            "Priority Recalculated",
            
            "Status Changed",
            "Task Started",
            "Task Completed", 
            "Task Reopened",
            "Task Archived",
            
            "Task Created",
            "Task Updated",
            "Task Deleted",
            
            "Field Updated",
            "Bulk Operation"
        };

        return Ok(actions);
    }

    [HttpGet("validate-action")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult ValidateAction([FromQuery] string action)
    {
        if (string.IsNullOrWhiteSpace(action))
        {
            return BadRequest(new { valid = false, message = "Ação não pode ser vazia" });
        }

        var isValid = _auditLogService.IsValidAction(action);
        var normalizedAction = _auditLogService.NormalizeAction(action);

        return Ok(new 
        { 
            valid = isValid,
            originalAction = action,
            normalizedAction = normalizedAction
        });
    }

    #endregion
}
