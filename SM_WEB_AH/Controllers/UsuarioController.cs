using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Models;
using System.Net;
using static System.Net.WebRequestMethods;

namespace SM_WEB_AH.Controllers
{
    public class UsuarioController(
           IHttpClientFactory _http,
           IConfiguration _config) : Controller
    {
        #region Cambiar Contraseña

        [HttpGet]
        public IActionResult Configuracion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarContrasenna(UsuarioModel model)
        {
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
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }

            throw new Exception("Ocurrió un error al intentar cambiar su contraseña de acceso.");
        }

        #endregion
    }
}
