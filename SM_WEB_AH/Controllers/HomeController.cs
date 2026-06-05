using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Models;
using System.Diagnostics;

namespace SM_WEB_AH.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        #region Registrar Usuarios

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(UsuarioModel model)
        {
            return View();
        }

        #endregion

        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        public IActionResult Principal()
        {
            return View();
        }
    }
}
