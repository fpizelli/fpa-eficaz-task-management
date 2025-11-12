using EficazAPI.Application.UseCases.Tasks;
using EficazAPI.Application.UseCases.Comments;
using EficazAPI.Application.UseCases.Users;
using EficazAPI.Application.UseCases.AuditLogs;
using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EficazAPI.WebApi.Controllers.AuditLogs
{
    [ApiController]
    [Route("api/audit")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class AuditLogController : ControllerBase
    {
        private readonly GetAuditLogsByTaskUseCase _getAuditLogs;

        public AuditLogController(GetAuditLogsByTaskUseCase getAuditLogs)
        {
            _getAuditLogs = getAuditLogs;
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult> GetByTask(Guid taskId)
        {
            try
            {
                var auditLogs = await _getAuditLogs.ExecuteAsync(taskId);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("task/{taskId}/paged")]
        public async Task<ActionResult> GetByTaskPaged(
            Guid taskId, 
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var auditLogs = await _getAuditLogs.ExecutePagedAsync(taskId, page, pageSize);
                return Ok(auditLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
