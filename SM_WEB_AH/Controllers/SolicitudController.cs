using Microsoft.AspNetCore.Mvc;
using SM_WEB_AH.Filters;
using SM_WEB_AH.Models;
using System.Net;
using System.Net.Http.Headers;

namespace SM_WEB_AH.Controllers
{
    [SessionAuthorize]
    public class SolicitudController(
        IHttpClientFactory _http,
        IConfiguration _config,
        IWebHostEnvironment _env) : Controller
    {

        [HttpGet]
        public IActionResult Bandeja()
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var urlApi = _config["Valores:UrlApi"] + "Solicitud/ConsultarSolicitudesUsuarioAPI";
            var response = client.GetAsync(urlApi).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var datos = response.Content.ReadFromJsonAsync<List<SolicitudModel>>().Result;
                return View(datos);
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                ViewBag.MensajeSeguridad = response.Content.ReadAsStringAsync().Result;
                ViewBag.ClaseMensajeSeguridad = "danger";
                return View(new List<SolicitudModel>());
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Home");
            }

            throw new Exception("Error al consultar las solicitudes.");
        }

        #region Registrar Solicitudes

        [HttpGet]
        public IActionResult AgregarSolicitud()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgregarSolicitud(SolicitudModel model, IFormFile Imagen)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var urlApi = _config["Valores:UrlApi"] + "Solicitud/RegistrarSolicitudAPI";
            var response = client.PostAsJsonAsync(urlApi, model).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var consecutivoSolicitud = response.Content.ReadFromJsonAsync<int>().Result;

                var carpeta = Path.GetFullPath(Path.Combine(_env.ContentRootPath, "..", "Storage", "pdfs"));
                GuardarPDF(Imagen, consecutivoSolicitud, carpeta);

                return RedirectToAction("Bandeja", "Solicitud");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                return View();
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Salir", "Home");
            }

            throw new Exception("Ocurrió un error al intentar registrar la solicitud.");
        }

        #endregion

        [HttpPost]
        public IActionResult CancelarSolicitud(int consecutivoSolicitud)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + HttpContext.Session.GetString("Token"));
            var urlApi = _config["Valores:UrlApi"] + "Solicitud/CancelarSolicitudAPI?consecutivoSolicitud=" + consecutivoSolicitud;
            var response = client.DeleteAsync(urlApi).Result;

            return Json(response.Content.ReadAsStringAsync().Result);
        }

        [HttpPost]
        public IActionResult AtenderSolicitud(int consecutivo, string solucion)
        {
            using var client = _http.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("Token"));
            var url = _config["Valores:UrlApi"] + "Solicitud/AtenderSolicitudAPI";
            var response = client.PutAsJsonAsync(url, new { ConsecutivoSolicitud = consecutivo, Solucion = solucion }).Result;

            return Json(response.Content.ReadAsStringAsync().Result);
        }

        private static void GuardarPDF(IFormFile Imagen, int ConsecutivoSolicitud, string carpeta)
        {
            Directory.CreateDirectory(carpeta);

            var ruta = Path.Combine(carpeta, $"{ConsecutivoSolicitud}.pdf");

            using var stream = new FileStream(ruta, FileMode.Create);
            Imagen.CopyTo(stream);
        }
    }
}