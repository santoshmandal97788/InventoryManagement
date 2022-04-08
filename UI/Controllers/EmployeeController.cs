using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models;
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

        public EmployeeController(IEmployeeRepository mockEmployeeRepository,IListItemRepository mockListItemRepository, IPersonRepository mockPersonRepository )
        {
            _mockEmployeeRepository = mockEmployeeRepository;
            _mockPersonRepository = mockPersonRepository;
            _mockListItemRepository = mockListItemRepository;
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
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
            ViewBag.Gender = genderList;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmployee(EmployeeViewModel model)
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
                    //Password = Helper.HashPassword(model.ConfirmPassword),
                    //random generating password
                    RoleId = model.RoleId,

                };
                Person person = new Person
                {
                    FirstName = model.FirstName,
                    MiddleName= model.MiddleName,
                    LastName = model.LastName,
                    GenderListItemId = model.GenderId
                };
                _mockEmployeeRepository.EmployeeIns(emp,person);

                return RedirectToAction("Index", "Employee");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        
        {
            Employee employee = _mockEmployeeRepository.GetEmployee(id);
            if (employee == null)
            {
                return View("NotFound", id);
            }
            var genderList = _mockListItemRepository.GetAllListItemById(1).Select(p => new SelectListItem
            {
                Value = p.ListItemId.ToString(),
                Text = p.ListItemName
            }).ToList();
            ViewBag.Gender = genderList;
            EmployeeViewModel employeeViewModel = new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                PersonId  = employee.PersonId,
                Email = employee.Email,
                FirstName= employee.Person.FirstName,
                MiddleName = employee.Person.MiddleName,
                LastName = employee.Person.LastName,
                GenderId= employee.Person.GenderListItemId,
                Gender = employee.Person.ListItem.ListItemName,
                RoleId = employee.RoleId,
                RoleName= employee.Role.RoleName
            };
            return View(employeeViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isexists = _mockEmployeeRepository.IsEmailInUse(model.Email);
                Employee emp = _mockEmployeeRepository.GetEmployee(model.EmployeeId);
                Person ppl =  _mockPersonRepository.GetPerson(model.PersonId);

                if (isexists && emp.Email != model.Email)
                {
                    ModelState.AddModelError("Emai", "Email Already Exists");
                    return View(model);
                }
                emp.Email = model.Email;
                emp.RoleId = model.RoleId;

                ppl.FirstName = model.FirstName;
                ppl.MiddleName = model.MiddleName;
                ppl.LastName = model.LastName;
                ppl.GenderListItemId = model.GenderId;

               _mockEmployeeRepository.EmployeeUpd(emp, ppl);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _mockEmployeeRepository.GetEmployeeDetails(id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
           // EmployeeViewModel employeeViewModel = new EmployeeViewModel() { EmployeeId = employee.EmployeeId, PersonId = employee.PersonId, FirstName = employee.Person.FirstName, MiddleName= employee.Person.MiddleName, LastName= employee.Person.LastName, Gender= employee.Person.ListItem.ListItemName, Email= employee.Email, RoleId= employee.RoleId, RoleName= employee.Role.RoleName};
            return View(employee);

        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
           
            Employee employeeToDelete = _mockEmployeeRepository.GetEmployee(id);
            if (employeeToDelete != null)
            {
                _mockEmployeeRepository.EmployeeDel(id);
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            return Json(new { success = false, message = "Something Went Wrong" });
        }
    }
}
