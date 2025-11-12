using EficazAPI.Domain.Enums;

namespace EficazAPI.Application.Services.Auth
{
    public class PermissionService : IPermissionService
    {
        public bool CanViewUsers(Role role)
        {
            return true;
        }

        public bool CanCreateUsers(Role role)
        {
            return role == Role.Gerente;
        }

        public bool CanEditUsers(Role role)
        {
            return role == Role.Gerente;
        }

        public bool CanDeleteUsers(Role role)
        {
            return role == Role.Gerente;
        }

        public bool CanEditUserRoles(Role role)
        {
            return role == Role.Gerente;
        }

        public bool CanCreateTasks(Role role)
        {
            return true;
        }

        public bool CanEditTasks(Role role)
        {
            return true;
        }

        public bool CanDeleteTasks(Role role)
        {
            return role == Role.Gerente;
        }

        public bool CanViewReports(Role role)
        {
            return true;
        }

        public bool IsAdmin(Role role)
        {
            return role == Role.Gerente;
        }
    }
}
