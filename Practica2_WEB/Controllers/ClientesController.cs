using Microsoft.AspNetCore.Mvc;
using Practica2_WEB.Models;
using Practica2_WEB.Services;

namespace Practica2_WEB.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.GetAllClientesAsync();
            return View(clientes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cedula,Nombre,Correo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var success = await _clienteService.CreateClienteAsync(cliente);
                if (success)
                {
                    TempData["Mensaje"] = "Cliente registrado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al registrar el cliente");
                }
            }
            return View(cliente);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _clienteService.GetClienteByIdAsync(id.Value);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdCliente,Cedula,Nombre,Correo,Estado")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
                return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _clienteService.UpdateClienteAsync(id, cliente);
                if (success)
                {
                    TempData["Mensaje"] = "Cliente actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar el cliente");
                }
            }
            return View(cliente);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
                return NotFound();

            var cliente = await _clienteService.GetClienteByIdAsync(id.Value);
            if (cliente == null)
                return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var success = await _clienteService.DeleteClienteAsync(id);
            if (success)
            {
                TempData["Mensaje"] = "Cliente eliminado exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
