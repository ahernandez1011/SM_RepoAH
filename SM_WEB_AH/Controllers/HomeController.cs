using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Models;
using System.Diagnostics;

namespace SM_WEB_AH.Controllers
{
    public class HomeController (IHttpClientFactory _http) : Controller
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
            using var cliente = _http.CreateClient();
            
            var urlApi = "https://localhost:7201/api/Home/RegistroAPI";
            var response = cliente.PostAsJsonAsync(urlApi, model).Result;

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
