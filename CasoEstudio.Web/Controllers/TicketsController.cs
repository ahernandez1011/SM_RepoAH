using Microsoft.AspNetCore.Mvc;
using CasoEstudio.Web.Data;
using CasoEstudio.Web.Models;

namespace CasoEstudio.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _repo;
        public TicketsController(ITicketRepository repo) { _repo = repo; }

        public async Task<IActionResult> Index()
        {
            var tickets = await _repo.GetTicketsAsync();
            return View(tickets);
        }

        public async Task<IActionResult> Create()
        {
            var tipos = await _repo.GetTiposAsync();
            ViewBag.Tipos = tipos;
            var model = new Ticket { FechaIngreso = DateTime.Now };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket model)
        {
            System.Diagnostics.Debug.WriteLine("=== CREATE POST INICIADO ===");

            // Verificar qué viene en el Request.Form PRIMERO
            System.Diagnostics.Debug.WriteLine("=== FORM DATA RAW ===");
            foreach (var key in Request.Form.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"{key} = {Request.Form[key]}");
            }

            System.Diagnostics.Debug.WriteLine($"PlacaVehiculo: '{model.PlacaVehiculo}' (IsNullOrEmpty: {string.IsNullOrEmpty(model.PlacaVehiculo)})");
            System.Diagnostics.Debug.WriteLine($"TipoVehiculo: {model.TipoVehiculo}");
            System.Diagnostics.Debug.WriteLine($"MontoTotal: {model.MontoTotal}");
            System.Diagnostics.Debug.WriteLine($"FechaIngreso (antes): {model.FechaIngreso}");

            // FechaIngreso se establece en el servidor y no se muestra en el formulario
            model.FechaIngreso = DateTime.Now;
            // Eliminar la entrada previa del ModelState para FechaIngreso para evitar validación del cliente/servidor
            ModelState.Remove(nameof(model.FechaIngreso));

            // Debug: Mostrar errores de validación
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("=== MODEL STATE INVÁLIDO ===");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state != null && state.Errors.Count > 0)
                    {
                        foreach (var error in state.Errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");
                        }
                    }
                }
                ViewBag.Tipos = await _repo.GetTiposAsync();
                return View(model);
            }

            System.Diagnostics.Debug.WriteLine("=== MODEL STATE VÁLIDO, INTENTANDO INSERTAR ===");
            var (code, message) = await _repo.InsertTicketAsync(model);
            System.Diagnostics.Debug.WriteLine($"Resultado InsertTicket - Code: {code}, Message: {message}");

            if (code != 0)
            {
                System.Diagnostics.Debug.WriteLine("=== ERROR AL INSERTAR ===");
                ModelState.AddModelError(string.Empty, message);
                ViewBag.Tipos = await _repo.GetTiposAsync();
                return View(model);
            }

            System.Diagnostics.Debug.WriteLine("=== INSERCIÓN EXITOSA, REDIRIGIENDO ===");
            return RedirectToAction(nameof(Index));
        }
    }
}
