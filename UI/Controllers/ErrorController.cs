using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound(int id, string msg)
        {
            ViewBag.Message = msg;
            return View();
        }
    }
}
