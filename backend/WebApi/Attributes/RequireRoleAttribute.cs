using EficazAPI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EficazAPI.WebApi.Attributes
{
    public class RequireRoleAttribute : ActionFilterAttribute
    {
        private readonly Role[] _allowedRoles;

        public RequireRoleAttribute(params Role[] allowedRoles)
        {
            _allowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;

            Console.WriteLine($"[ROLE AUTH] Verificando autorização - IsAuthenticated: {user.Identity?.IsAuthenticated}");
            Console.WriteLine($"[ROLE AUTH] Claims: {string.Join(", ", user.Claims.Select(c => $"{c.Type}={c.Value}"))}");

            if (!user.Identity?.IsAuthenticated == true)
            {
                Console.WriteLine("[ROLE AUTH] Usuário não autenticado");
                context.Result = new UnauthorizedObjectResult(new { message = "Usuário não autenticado" });
                return;
            }

            var roleClaim = user.FindFirst("role")?.Value;
            Console.WriteLine($"[ROLE AUTH] Role claim encontrado: '{roleClaim}'");
            Console.WriteLine($"[ROLE AUTH] Roles permitidos: {string.Join(", ", _allowedRoles)}");
            
            if (string.IsNullOrEmpty(roleClaim) || !Enum.TryParse<Role>(roleClaim, out var userRole))
            {
                Console.WriteLine($"[ROLE AUTH] ERRO: Role inválido ou não encontrado. RoleClaim: '{roleClaim}'");
                context.Result = new ForbidResult();
                return;
            }

            Console.WriteLine($"[ROLE AUTH] Role do usuário: {userRole}");

            if (!_allowedRoles.Contains(userRole))
            {
                Console.WriteLine($"[ROLE AUTH] ERRO: Role {userRole} não está nos roles permitidos: {string.Join(", ", _allowedRoles)}");
                context.Result = new ForbidResult();
                return;
            }

            Console.WriteLine($"[ROLE AUTH] SUCCESS: Usuário autorizado com role {userRole}");
            base.OnActionExecuting(context);
        }
    }
}
