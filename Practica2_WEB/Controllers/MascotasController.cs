using Microsoft.AspNetCore.Mvc;
using Practica2_WEB.Models;
using Practica2_WEB.Services;

namespace Practica2_WEB.Controllers
{
    public class MascotasController : Controller
    {
        private readonly IMascotaService _mascotaService;
        private readonly IClienteService _clienteService;

        public MascotasController(IMascotaService mascotaService, IClienteService clienteService)
        {
            _mascotaService = mascotaService;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Consulta()
        {
            var mascotas = await _mascotaService.GetAllMascotasAsync();
            return View(mascotas);
        }

        public async Task<IActionResult> Create()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            ViewBag.Clientes = clientes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Especie,Raza,Peso,IdCliente")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
                var success = await _mascotaService.CreateMascotaAsync(mascota);
                if (success)
                {
                    TempData["Mensaje"] = "Mascota registrada exitosamente";
                    return RedirectToAction(nameof(Consulta));
                }
                else
                {
                    ModelState.AddModelError("", "Error al registrar la mascota. Verifique que no haya más de 2 mascotas de la misma especie para este cliente");
                }
            }

            var clientes = await _clienteService.GetAllClientesAsync();
            ViewBag.Clientes = clientes;
            return View(mascota);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
                return NotFound();

            var mascota = await _mascotaService.GetMascotaByIdAsync(id.Value);
            if (mascota == null)
                return NotFound();

            var clientes = await _clienteService.GetAllClientesAsync();
            ViewBag.Clientes = clientes;
            return View(mascota);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdMascota,Nombre,Especie,Raza,Peso,IdCliente")] Mascota mascota)
        {
            if (id != mascota.IdMascota)
                return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _mascotaService.UpdateMascotaAsync(id, mascota);
                if (success)
                {
                    TempData["Mensaje"] = "Mascota actualizada exitosamente";
                    return RedirectToAction(nameof(Consulta));
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar la mascota");
                }
            }

            var clientes = await _clienteService.GetAllClientesAsync();
            ViewBag.Clientes = clientes;
            return View(mascota);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var mascota = await _mascotaService.GetMascotaByIdAsync(id.Value);
            if (mascota == null)
                return NotFound();

            return View(mascota);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var success = await _mascotaService.DeleteMascotaAsync(id);
            if (success)
            {
                TempData["Mensaje"] = "Mascota eliminada exitosamente";
            }
            return RedirectToAction(nameof(Consulta));
        }
    }
}
