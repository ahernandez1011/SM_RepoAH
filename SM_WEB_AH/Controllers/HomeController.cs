using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Models;
using System.Net;

namespace SM_WEB_AH.Controllers
{
    public class HomeController (
        IHttpClientFactory _http,
        IConfiguration _config) : Controller
    {

        #region Iniciar Sesión

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UsuarioModel model)
        {
            using var client = _http.CreateClient();

            var urlApi = _config["Valores:UrlApi"] + "Home/IniciarSesionAPI";
            var response = client.PostAsJsonAsync(urlApi, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Principal", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                //Agregar mensaje de error a la vista
                return View();
            }

            throw new Exception("Ocurrió un error al intentar iniciar sesión.");
        }

        #endregion

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
