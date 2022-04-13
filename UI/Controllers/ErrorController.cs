using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace UI.Controllers
{
    public class ErrorController : Controller
    {
        // If there is 404 status code, the route path will become Error/404
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                
                    return View("404 NotFound");

                case 500:
                    ViewBag.ErrorMessage = "Internal Server Error";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                    return View("500 InternalServerError");                  
            }
            return View("404 NotFound");


        }

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


        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            // Retrieve the exception Details
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            return View("Error");
        }
    }
}
