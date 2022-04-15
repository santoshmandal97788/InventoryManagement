using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Services.IService;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDashboardService dashboardService, ILogger<HomeController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }
        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var result = _dashboardService.TotalEmployeeCount();
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while getting employee count record", ex.Message);
                throw;
            }
          
        }
    }
}
