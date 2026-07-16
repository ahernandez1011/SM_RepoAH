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
                var datos = response.Content.ReadFromJsonAsync<UsuarioModel>().Result;

                HttpContext.Session.SetString("Autenticado", "1");
                HttpContext.Session.SetString("Nombre", datos!.Nombre);
                if (datos.IndicadorTemp)
                    return RedirectToAction("Configuracion", "Usuario");

                return RedirectToAction("Principal", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
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
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }

            throw new Exception("Ocurrió un error al intentar registrar el usuario.");
        }

        #endregion

        #region Recuperar Acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(UsuarioModel model)
        {
            using var client = _http.CreateClient();

            var urlApi = _config["Valores:UrlApi"] + "Home/RecuperarAccesoAPI";
            var response = client.PostAsJsonAsync(urlApi, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound
                || response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }

            throw new Exception("Ocurrió un error al intentar recuperar su acceso.");
        }

        #endregion

        #region Cerrar Sesión

        [HttpGet]
        public IActionResult Salir()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        [HttpGet]
        public IActionResult Principal()
        {
            return View();
        }
    }
}