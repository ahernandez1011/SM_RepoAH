using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Filters;
using SM_WEB_AH.Models;
using System.Net;
using static System.Net.WebRequestMethods;

namespace SM_WEB_AH.Controllers
{
    [SessionAuthorize]
    public class ContactoController(
            IHttpClientFactory _http,
            IConfiguration _config) : Controller
    {
        public IActionResult Chat()
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var urlApi = _config["Valores:UrlApi"] + "Contacto/ConsultarSolicitudesChatAPI";
            var response = client.GetAsync(urlApi).Result;

            List<SolicitudChatModel> solicitudes = [];

            if (response.StatusCode == HttpStatusCode.OK)
            {
                solicitudes = response.Content.ReadFromJsonAsync<List<SolicitudChatModel>>().Result ?? [];
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Home");
            }

            ViewBag.Token = HttpContext.Session.GetString("Token");
            ViewBag.UrlHub = _config["Valores:UrlHub"];
            ViewBag.ConsecutivoUsuario = HttpContext.Session.GetInt32("Consecutivo");
            return View(solicitudes);
        }
    }
}
