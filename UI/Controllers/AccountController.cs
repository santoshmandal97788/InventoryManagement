using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using UI.ViewModel;
using UI.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UI.Services.IService;
using UI.Models;

namespace UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmployeeRepository _mockEmployeeRepository;

        public AccountController(IAccountService accountService, IEmployeeRepository mockEmployeeRepository , IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _mockEmployeeRepository = mockEmployeeRepository;
        }
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
              //ViewBag.Message = TempData["message"];  
               return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                var user =  _accountService.AuthenticateUser(model.Email, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ViewBag.Error = "Invalid login attempt.";
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim("Id", user.EmployeeId.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("FullName", string.Concat(user.Person.FirstName, " ", user.Person.MiddleName, " ",
                                                       user.Person.LastName)),
                    new Claim(ClaimTypes.Role, user.Role.RoleName),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                   
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Administration");
                }
            }

            // Something failed. Redisplay the form.
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Account");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                int loggedInUserId = Convert.ToInt32(id);

                var employee =  _mockEmployeeRepository.GetEmployeeForPasswordReset(loggedInUserId);
                if (employee.Password !=Helper.HashPassword(model.CurrentPassword))
                {
                    ViewBag.Error = "Current Password does not match.";
                    return View(model);
                }
                employee.Password = Helper.HashPassword(model.ConfirmPassword);
                var result = _accountService.ChangePassword(employee);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                TempData["message"] = "Password Changed Successfully.";
                return RedirectToAction("Login", "Account");

            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ConfirmEmail()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  IActionResult ConfirmEmail(ResetPasswordConfirmEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _accountService.GetEmployeeByEmail(model.Email);
                if (employee == null)
                {
                    ViewBag.Error = "Email does not match.";
                    return View(model);
                }
                return RedirectToAction("ResetPassword", "Account", new { id = employee.EmployeeId });
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(int id, ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mockEmployeeRepository.GetEmployee(id);
                if (employee == null)
                {
                    ViewBag.Error = "Something Went Wrong. Try Again Later!.";
                    return View(model);
                }
                employee.Password = Helper.HashPassword(model.ConfirmPassword);
                var result = _accountService.ChangePassword(employee);
                TempData["message"] = "Password Reset Successfully.";
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
    }
}
