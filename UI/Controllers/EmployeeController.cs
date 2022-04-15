using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models;
using UI.Security;
using UI.Services;
using UI.Services.IService;
using UI.ViewModel;

namespace UI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _mockEmployeeRepository;
        private readonly IPersonRepository _mockPersonRepository;
        private readonly IListItemRepository _mockListItemRepository;
        private readonly IRoleRepository _mockRoleRepository;
        private readonly IDataProtector protector;
        private readonly ILogger<EmployeeController> _logger;


        public EmployeeController(ILogger<EmployeeController> logger, 
                                    IEmployeeRepository mockEmployeeRepository,
                                    IListItemRepository mockListItemRepository,
                                    IPersonRepository mockPersonRepository,
                                    IRoleRepository mockRoleRepository,
                                    IDataProtectionProvider dataProtectionProvider,
                                    DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _mockEmployeeRepository = mockEmployeeRepository;
            _mockPersonRepository = mockPersonRepository;
            _mockListItemRepository = mockListItemRepository;
            _mockRoleRepository = mockRoleRepository;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
            _logger = logger;
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsEmailExists(string email, int employeeId)
        {
            bool isExists = false;
            if (employeeId == 0)
            {
                isExists = _mockEmployeeRepository.IsEmailInUse(email);
            }
            else
            {
                isExists = _mockEmployeeRepository.IsEmailInUse(email);
                var employeeById = _mockEmployeeRepository.GetEmployee(employeeId);
                if (isExists && employeeById.Email != email)
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
                return Json($"Email : {email}  already exists in Database.");
            }
            else
            {
                return Json(true);
            }

        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public IActionResult GetAllEmployees()
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
                var jsonData = _mockEmployeeRepository.GetListEmployeesForDataTable(draw, start, length, sortColumn, sortColumnDirection, searchValue, pageSize, skip);
                return Ok(jsonData.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            var genderList = _mockListItemRepository.GetAllListItemById(1).Select(p => new SelectListItem
            {
                Value = p.ListItemId.ToString(),
                Text = p.ListItemName
            }).ToList();
            var roleList = _mockRoleRepository.GetAllRoles().Select(p => new SelectListItem
            {
                Value = p.RoleId.ToString(),
                Text = p.RoleName
            }).ToList();
            ViewBag.Gender = genderList;
            ViewBag.Role = roleList;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(EmployeeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isexists = _mockEmployeeRepository.IsEmailInUse(model.Email);
                    if (isexists)
                    {
                        ModelState.AddModelError("Email", "Email Already Exists");
                        return View(model);
                    }

                    Employee emp = new Employee
                    {
                        Email = model.Email,
                        Password = Helper.HashPassword(model.ConfirmPassword),
                        //random generating password
                        RoleId = model.RoleId,

                    };
                    Person person = new Person
                    {
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        GenderListItemId = model.GenderId
                    };
                    _mockEmployeeRepository.EmployeeIns(emp, person);
                    TempData["message"] = "Employee Added Successfully";
                    return RedirectToAction("Index", "Employee");
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while adding employee", ex.Message);
                throw ex;
            }
           
        }
        [HttpGet]
        [Authorize]
        public IActionResult Edit(string id)
        
        {
            try
            {
                int employeeId = Convert.ToInt32(protector.Unprotect(id));
                Employee employee = _mockEmployeeRepository.GetEmployee(employeeId);
                if (employee == null)
                {
                    string msg = $"Employee with id: {id}, you are looking cannot be found";
                    TempData["errMessage"] = msg;
                    return RedirectToAction("NotFound", "Error");
                }
                var genderList = _mockListItemRepository.GetAllListItemById(1).Select(p => new SelectListItem
                {
                    Value = p.ListItemId.ToString(),
                    Text = p.ListItemName
                }).ToList();
                var roleList = _mockRoleRepository.GetAllRoles().Select(p => new SelectListItem
                {
                    Value = p.RoleId.ToString(),
                    Text = p.RoleName
                }).ToList();

                ViewBag.Gender = genderList;
                ViewBag.Role = roleList;
                EmployeeViewModel employeeViewModel = new EmployeeViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    PersonId = employee.PersonId,
                    Email = employee.Email,
                    FirstName = employee.Person.FirstName,
                    MiddleName = employee.Person.MiddleName,
                    LastName = employee.Person.LastName,
                    GenderId = employee.Person.GenderListItemId,
                    Gender = employee.Person.ListItem.ListItemName,
                    RoleId = employee.Role.RoleId,
                    RoleName = employee.Role.RoleName
                };
                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting employee data for editing.", ex.Message);
                throw ex;
            }

     
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isexists = _mockEmployeeRepository.IsEmailInUse(model.Email);
                    Employee emp = _mockEmployeeRepository.GetEmployee(model.EmployeeId);
                    Person ppl = _mockPersonRepository.GetPerson(model.PersonId);

                    if (isexists && emp.Email != model.Email)
                    {
                        ModelState.AddModelError("Email", "Email Already Exists");
                        return View(model);
                    }
                    emp.Email = model.Email;
                    emp.RoleId = model.RoleId;

                    ppl.FirstName = model.FirstName;
                    ppl.MiddleName = model.MiddleName;
                    ppl.LastName = model.LastName;
                    ppl.GenderListItemId = model.GenderId;

                    _mockEmployeeRepository.EmployeeUpd(emp, ppl);
                    TempData["message"] = "Employee Updated Successfully";
                    return RedirectToAction("Index");
                }
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error while updating employee data.", ex.Message);
                throw;
            }
           
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(string id)
        {
            try
            {
                int employeeId = Convert.ToInt32(protector.Unprotect(id));
                var employee = _mockEmployeeRepository.GetEmployeeDetails(employeeId);
                if (employee == null)
                {
                    Response.StatusCode = 404;
                    string msg = $"Employee with id: {id}, you are looking cannot be found";
                    TempData["errMessage"] = msg;
                    return RedirectToAction("NotFound", "Error");
                }
                // EmployeeViewModel employeeViewModel = new EmployeeViewModel() { EmployeeId = employee.EmployeeId, PersonId = employee.PersonId, FirstName = employee.Person.FirstName, MiddleName= employee.Person.MiddleName, LastName= employee.Person.LastName, Gender= employee.Person.ListItem.ListItemName, Email= employee.Email, RoleId= employee.RoleId, RoleName= employee.Role.RoleName};
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to view Employee Details. Invalid EncryptedEmployeeId: {id}", ex.Message);
                throw;
            }
           

        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(string id)
        {
            try
            {
                int employeeId = Convert.ToInt32(protector.Unprotect(id));
                Employee employeeToDelete = _mockEmployeeRepository.GetEmployee(employeeId);
                if (employeeToDelete != null)
                {
                    _mockEmployeeRepository.EmployeeDel(employeeId);
                    return Json(new { success = true, message = "Deleted Successfully" });
                }
                else
                {
                    Response.StatusCode = 404;
                    string msg = $"Employee with id: {id}, you are looking cannot be found";
                    TempData["errMessage"] = msg;
                    return RedirectToAction("NotFound", "Error");
                }
                return Json(new { success = false, message = "Something Went Wrong" });

            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to Delete Employee. Invalid EncryptedRoleId: {id}", ex.Message);
                throw;
            }
           
        }
    }
}
