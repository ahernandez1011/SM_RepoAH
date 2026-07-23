using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Filters;
using SM_WEB_AH.Models;
using System.Net;

namespace SM_WEB_AH.Controllers
{
    [SessionAuthorize]
    public class UsuarioController(
           IHttpClientFactory _http,
           IConfiguration _config) : Controller
    {
        #region Cambiar Contraseña y Perfil

        [HttpGet]
        public IActionResult Configuracion()
        {
            var consecutivo = HttpContext.Session.GetInt32("Consecutivo")!.Value;
            using var client = _http.CreateClient();

            var urlApi = _config["Valores:UrlApi"] + "Usuario/ConsultarUsuarioAPI?Consecutivo=" + consecutivo;
            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content.ReadFromJsonAsync<UsuarioModel>().Result;
                return View(datos);
            }

            throw new Exception("Ocurrió un error al intentar cambiar su contraseña de acceso.");
        }
        [HttpPost]
        public IActionResult CambiarContrasenna(UsuarioModel model)
        {
            model.Contrasenna = BCrypt.Net.BCrypt.HashPassword(model.Contrasenna);

            model.Consecutivo = HttpContext.Session.GetInt32("Consecutivo")!.Value;

            using var client = _http.CreateClient();

            var urlApi = _config["Valores:UrlApi"] + "Usuario/CambiarContrasennaAPI";
            var response = client.PutAsJsonAsync(urlApi, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return RedirectToAction("Salir", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.MensajeSeguridad = response.Content.ReadAsStringAsync().Result;
                ViewBag.ClaseMensajeSeguridad = "danger";
                return View("Configuracion", model);
            }

            throw new Exception("Ocurrió un error al intentar cambiar su contraseña de acceso.");
        }
        [HttpPost]
        public IActionResult CambiarPerfil(UsuarioModel model)
        {
            model.Consecutivo = HttpContext.Session.GetInt32("Consecutivo")!.Value;

            using var client = _http.CreateClient();

            var urlApi = _config["Valores:UrlApi"] + "Usuario/CambiarPerfilAPI";
            var response = client.PutAsJsonAsync(urlApi, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContext.Session.SetString("Nombre", model.Nombre);
                ViewBag.MensajePerfil = response.Content.ReadAsStringAsync().Result;
                ViewBag.ClaseMensajePerfil = "success";
                return View("Configuracion", model);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.MensajePerfil = response.Content.ReadAsStringAsync().Result;
                ViewBag.ClaseMensajePerfil = "danger";
                return View("Configuracion", model);
            }

            throw new Exception("Ocurrió un error al intentar cambiar sus datos personales.");
        }
        #endregion
    }
}
