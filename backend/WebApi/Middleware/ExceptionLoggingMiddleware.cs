using System.Text.Json;

namespace EficazAPI.WebApi.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                
                var response = new { error = "Something went wrong" };
                var json = JsonSerializer.Serialize(response);
                
                await context.Response.WriteAsync(json);
            }
        }
    }
}
