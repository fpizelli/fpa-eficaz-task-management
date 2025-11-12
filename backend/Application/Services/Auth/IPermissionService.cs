using EficazAPI.Domain.Enums;

namespace EficazAPI.Application.Services.Auth
{
    public interface IPermissionService
    {
        bool CanViewUsers(Role role);

        bool CanCreateUsers(Role role);

        bool CanEditUsers(Role role);

        bool CanDeleteUsers(Role role);

        bool CanEditUserRoles(Role role);

        bool CanCreateTasks(Role role);

        bool CanEditTasks(Role role);

        bool CanDeleteTasks(Role role);

        bool CanViewReports(Role role);

        bool IsAdmin(Role role);
    }
}
