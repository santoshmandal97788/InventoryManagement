using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Services.IService;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [Authorize]
        public IActionResult Index()
        {
            var result = _dashboardService.TotalEmployeeCount();
            return View(result);
        }
    }
}
