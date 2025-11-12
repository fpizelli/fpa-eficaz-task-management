using EficazAPI.Application.DTOs.Tasks;
using EficazAPI.Application.DTOs.Comments;
using EficazAPI.Application.DTOs.Users;
using EficazAPI.Application.DTOs.AuditLogs;
using EficazAPI.Application.DTOs.Auth;
using EficazAPI.Application.DTOs.Shared;
using EficazAPI.Application.UseCases.Tasks;
using EficazAPI.Application.UseCases.Comments;
using EficazAPI.Application.UseCases.Users;
using EficazAPI.Application.UseCases.AuditLogs;
using Microsoft.AspNetCore.Mvc;

namespace EficazAPI.WebApi.Controllers.Comments
{
    [ApiController]
    [Route("api/comments")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class CommentController : ControllerBase
    {
        private readonly AddCommentUseCase _addComment;
        private readonly GetCommentsByTaskUseCase _getComments;

        public CommentController(
            AddCommentUseCase addComment,
            GetCommentsByTaskUseCase getComments)
        {
            _addComment = addComment;
            _getComments = getComments;
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult> GetByTask(Guid taskId)
        {
            try
            {
                var comments = await _getComments.ExecuteAsync(taskId);
                return Ok(comments);
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
                var pagination = new PaginationParams(page, pageSize);
                var comments = await _getComments.ExecutePagedAsync(taskId, pagination);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateCommentDto dto)
        {
            try
            {
                var result = await _addComment.ExecuteAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
