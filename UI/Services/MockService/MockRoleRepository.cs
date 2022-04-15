using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using UI.Data;
using UI.Models;
using UI.Security;

namespace UI.Services
{
    public class MockRoleRepository : IRoleRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDataProtector protector;

        public MockRoleRepository(AppDbContext appDbContext, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _appDbContext = appDbContext;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
        #region ------Get All Roles List------
        public IEnumerable<Role> GetAllRoles()
        {
            var roleLst = from role in _appDbContext.Roles select role;
            //var roleLst = _appDbContext.Roles.ToList();
            return roleLst;
        }
        #endregion

        #region -----Get single role------
        public Role GetRole(int Id)
        {
            return _appDbContext.Roles.Where(r => r.RoleId == Id).FirstOrDefault();
        }
        #endregion


        public bool RoleAssignToEmpployee(int roleId)
        {
            throw new NotImplementedException();
        }

        #region -----Delete Role------
        public Role RoleDel(int id)
        {
            Role role = _appDbContext.Roles.SingleOrDefault(r => r.RoleId == id);
            if (role != null)
            {
                _appDbContext.Roles.Remove(role);
                _appDbContext.SaveChanges();
            }
            return role;
        }
        #endregion

        #region ------Check Role Name exist or Not-------
        public bool RoleExists(string roleName)
        {
            bool isRoleNameExists = _appDbContext.Roles.Any(r => r.RoleName == roleName);
            if (isRoleNameExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region -----Role insert------
        public Role RoleIns(Role role)
        {
            _appDbContext.Roles.Add(role);
            _appDbContext.SaveChanges();
            return role;
        }
        #endregion

        #region ----Update Role------
        public Role RoleUpd(Role role)
        {
            Role roleToUpdate = _appDbContext.Roles.FirstOrDefault(r=> r.RoleId == role.RoleId);
            if (roleToUpdate != null)
            {
                roleToUpdate.RoleName = role.RoleName;
                _appDbContext.Roles.Update(roleToUpdate);
                _appDbContext.SaveChanges();
            }
            return roleToUpdate;
        }
        #endregion

        #region ----Datatable get all roles ----
        public JsonResult GetRolesForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip)
        {
            int recordsTotal = 0;
            List<Role> roleList = new List<Role>();
            roleList= (from role in _appDbContext.Roles orderby role.RoleName select new Role
            {
                EncryptedId = protector.Protect(role.RoleId.ToString()),
               // RoleId = role.RoleId,
                RoleName = role.RoleName,
            }).Skip(skip).Take(pageSize).ToList();
            //roleList = _appDbContext.Roles.Skip(skip).Take(pageSize).ToList();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                roleList = roleList.OrderBy(s => sortColumn + " " + sortColumnDirection).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                roleList = roleList.Where(r => r.RoleId.ToString().Contains(searchValue.ToLower()) || r.RoleName.ToLower().Contains(searchValue.ToLower())).ToList();

            }
            recordsTotal = _appDbContext.Roles.Count();
            var data = roleList;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }
        #endregion
    }
}
