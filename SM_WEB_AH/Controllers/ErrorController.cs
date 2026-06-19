using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SM_WEB_AH.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult CapturarError()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View("Error");
        }
    }
}
