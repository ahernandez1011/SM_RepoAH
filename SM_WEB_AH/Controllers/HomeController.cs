using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace SM_WEB_AH.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
