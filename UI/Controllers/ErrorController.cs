using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace UI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            string message = TempData["errMessage"] as string;
            TempData.Keep("errMessage");
            var controllerName = "";
            if (message != null)
            {
                 controllerName = Regex.Replace(message.Split()[0], @"[^0-9a-zA-Z\ ]+", "");
                //if (controllerName == "Role" )
                //{
                //    controllerName = "Administration";
                //}

            }
            ViewBag.Message = message;
            ViewBag.ControllerName = controllerName;
            return View();
        }
    }
}
