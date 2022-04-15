using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Security;
using UI.Services;
using UI.ViewModel;

namespace UI.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdministrationController : Controller
    {
        private readonly IRoleRepository _mockRoleRepository;
        private readonly IDataProtector protector;
        private readonly ILogger<AdministrationController> _logger;

        public AdministrationController(ILogger<AdministrationController> logger, IRoleRepository mockRoleRepository, IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _mockRoleRepository = mockRoleRepository;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _logger = logger;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsRoleExists(string roleName, int roleId)
        {
            //bool isExists = _mockRoleRepository.RoleExists(roleName);
            //if (isExists)
            //{
            //    return Json($"Role Name: {roleName}  already exists in Database.");
            //}
            //else
            //{
            //    return Json(true);
            //}

            bool isExists = false;
            if (roleId == 0)
            {
                isExists = _mockRoleRepository.RoleExists(roleName);
            }
            else
            {
                isExists = _mockRoleRepository.RoleExists(roleName);
                var roleById = _mockRoleRepository.GetRole(roleId);
                if (isExists && roleById.RoleName != roleName)
                {
                    isExists = true;
                }
                else
                {
                    isExists = false;
                }

            }
            if (isExists)
            {
                    return Json($"Role Name: {roleName}  already exists in Database.");
            }
            else
            {
                return Json(true);
            }

        }
        [HttpGet]
        public IActionResult Index()
        { 
            return View();
                
        }
        [HttpPost]
        public IActionResult GetRole()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumn = Request.Form["order[0][column]"].FirstOrDefault();
                //var colName = Request.Form["columns[" + sortColumn + "][name]"].FirstOrDefault();
                //var colData = Request.Form["columns[" + sortColumn + "][data]"].FirstOrDefault();

                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var jsonData = _mockRoleRepository.GetRolesForDataTable(draw, start, length, sortColumn, sortColumnDirection, searchValue, pageSize, skip);
                
                return Ok(jsonData.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRole(RoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isexists = _mockRoleRepository.RoleExists(model.RoleName);
                    if (isexists)
                    {
                        ModelState.AddModelError("RoleName", "Role Already Exists");
                        return Json(new { isValid = false });
                    }

                    Role role = new Role
                    {
                        RoleName = model.RoleName
                    };
                    _mockRoleRepository.RoleIns(role);
                    TempData["message"] = "Role Added Successfully";
                    return RedirectToAction("Index", "Administration");
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
           
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                int roleId = Convert.ToInt32(protector.Unprotect(id));
                Role role = _mockRoleRepository.GetRole(roleId);
                if (role == null)
                {
                    string msg = $"Role with id: {id}, cannot be found";
                    TempData["errMessage"] = msg;
                    return RedirectToAction("NotFound", "Error");

                }
                RoleViewModel roleViewModel = new RoleViewModel
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                };
                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw ex;
            }
         
        }
        [HttpPost]
        public IActionResult Edit(RoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isexists = _mockRoleRepository.RoleExists(model.RoleName);
                    Role role = _mockRoleRepository.GetRole(model.RoleId);
                    if (isexists && role.RoleName != model.RoleName)
                    {
                        ModelState.AddModelError("RoleName", "Role Name Already Exist");
                        return View(model);
                    }
                    role.RoleName = model.RoleName;
                    Role updatedRole = _mockRoleRepository.RoleUpd(role);
                    TempData["message"] = "Role Updated Successfully";
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to Update Role", ex.Message);
                throw ex;
            }
          
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            try
            {
                int roleId = Convert.ToInt32(protector.Unprotect(id));
                var role = _mockRoleRepository.GetRole(roleId);
                if (role == null)
                {
                    Response.StatusCode = 404;
                    string msg = $"Role with id: {id}, cannot be found";
                    TempData["errMessage"] = msg;
                    return RedirectToAction("NotFound", "Error");
                }
                RoleViewModel roleViewModel = new RoleViewModel() { RoleId = role.RoleId, RoleName = role.RoleName };
                return View(roleViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to view Role Details. Invalid EncryptedRoleId: {id}", ex.Message);
                throw ex;
            }
          
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                int roleId = Convert.ToInt32(protector.Unprotect(id));
                Role roleToDelete = _mockRoleRepository.GetRole(roleId);
                if (roleToDelete != null)
                {
                    _mockRoleRepository.RoleDel(roleId);
                    return Json(new { success = true, message = "Deleted Successfully" });
                }
                else
                {
                    string msg = $"Role with id: {id}, you are looking cannot be found";
                    TempData["errMessage"] = msg;
                    return RedirectToAction("NotFound", "Error");
                }
                return Json(new { success = false, message = "Something Went Wrong" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to Delete Role. Invalid EncryptedRoleId: {id}", ex.Message);
                throw ex;
            }
           
        }
    }
}
