using Microsoft.AspNetCore.Mvc;

namespace EficazAPI.WebApi.Controllers.Shared
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult HandleException(Exception ex)
        {
            return ex switch
            {
                ArgumentException argEx => BadRequest(new { error = argEx.Message }),
                InvalidOperationException invEx => NotFound(new { error = invEx.Message }),
                UnauthorizedAccessException => Unauthorized(new { error = "Access denied" }),
                _ => StatusCode(500, new { error = "An internal error occurred" })
            };
        }

        protected ActionResult HandleSuccess<T>(T data, string? message = null)
        {
            var response = new
            {
                success = true,
                data,
                message
            };
            return Ok(response);
        }

        protected ActionResult HandleCreated<T>(T data, string actionName, object routeValues)
        {
            return CreatedAtAction(actionName, routeValues, new
            {
                success = true,
                data
            });
        }

        protected ActionResult HandleNoContent(string? message = null)
        {
            if (string.IsNullOrEmpty(message))
                return NoContent();

            return Ok(new { success = true, message });
        }
    }
}
