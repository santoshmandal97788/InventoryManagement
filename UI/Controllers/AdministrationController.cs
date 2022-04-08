using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Services;
using UI.ViewModel;

namespace UI.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AdministrationController : Controller
    {
        private readonly IRoleRepository _mockRoleRepository;

        public AdministrationController(IRoleRepository mockRoleRepository)
        {
            _mockRoleRepository = mockRoleRepository;
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
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumn = Request.Form["order[0][column]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var jsonData = _mockRoleRepository.GetRolesForDataTable(draw, start, length, sortColumn, sortColumnDirection, searchValue, pageSize, skip);
                return Ok(jsonData.Value);
            }
            catch (Exception ex)
            {
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Role role = _mockRoleRepository.GetRole(id);
            if (role == null)
            {
                return RedirectToAction("NotFound", "Error", id);

            }
            RoleViewModel roleViewModel = new RoleViewModel
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
            };
            return View(roleViewModel);
        }
        [HttpPost]
        public IActionResult Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isexists = _mockRoleRepository.RoleExists(model.RoleName);
                Role role = _mockRoleRepository.GetRole(model.RoleId);
                if (isexists && role.RoleName != model.RoleName)
                {
                    ModelState.AddModelError("RoleName", "Role Name Already Exist");
                    return  View(model);
                }
                role.RoleName = model.RoleName;               
                Role updatedRole = _mockRoleRepository.RoleUpd(role);
                TempData["message"] = "Role Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var role = _mockRoleRepository.GetRole(id);
            if (role == null)
            {
                Response.StatusCode = 404;
                return RedirectToAction("NotFound", "Error", id);
            }
            RoleViewModel roleViewModel = new RoleViewModel() { RoleId = role.RoleId, RoleName = role.RoleName };
            return View(roleViewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Role roleToDelete = _mockRoleRepository.GetRole(id);
            if (roleToDelete != null)
            {
                _mockRoleRepository.RoleDel(id);
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            else
            {
                return RedirectToAction("NotFound", "Error", id);
            }
            return Json(new { success = false, message = "Something Went Wrong" });
        }
    }
}
