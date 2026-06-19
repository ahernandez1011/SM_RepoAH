using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Models;

namespace SM_WEB_AH.Controllers
{
    public class HomeController (
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
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
            using var client = _http.CreateClient();
            
            var urlApi = _config["Valores:UrlApi"] + "Home/RegistroAPI";
            var response = client.PostAsJsonAsync(urlApi, model).Result;
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
