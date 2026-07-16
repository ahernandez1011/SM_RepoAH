using Microsoft.AspNetCore.Mvc;

namespace SM_WEB_AH.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Configuracion()
        {
            return View();
        }
    }
}
