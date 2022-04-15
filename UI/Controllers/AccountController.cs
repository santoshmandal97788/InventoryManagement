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
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService, IEmployeeRepository mockEmployeeRepository , IHttpContextAccessor httpContextAccessor)
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
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = _accountService.AuthenticateUser(model.Email, model.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ViewBag.Error = "Invalid login attempt.";
                    _logger.LogWarning($"Invalid login!! User with provided email:{model.Email} and password doesnot match");
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
                    return RedirectToAction("Index", "Home");
                }
            }

            // Something failed. Redisplay the form.
            return View(model);

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
               
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to Logout", ex.Message);
            }
            return RedirectToAction("Login", "Account");
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
                try
                {
                    var id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                    if (string.IsNullOrEmpty(id))
                    {
                        _logger.LogError("User must be logged in. Id is null");
                    }
                    int loggedInUserId = Convert.ToInt32(id);

                    var employee = _mockEmployeeRepository.GetEmployeeForPasswordReset(loggedInUserId);
                    if (employee.Password != Helper.HashPassword(model.CurrentPassword))
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
                catch (Exception ex)
                {
                    _logger.LogError("ex.message");
                   
                }
              

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
                    _logger.LogError($"Email:{model.Email} doesnot match in database");
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
                    _logger.LogError($"Employee with Id: {id} does not exists");
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
