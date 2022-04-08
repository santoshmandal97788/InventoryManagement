using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Services
{
    public interface IRoleRepository
    {
        Role GetRole(int Id);
        IEnumerable<Role> GetAllRoles();
        Role RoleIns(Role role);
        Role RoleUpd(Role role);
        Role RoleDel(int id);
        bool RoleExists(string roleName);
        bool RoleAssignToEmpployee(int roleId);
        JsonResult GetRolesForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip);

    }
}
