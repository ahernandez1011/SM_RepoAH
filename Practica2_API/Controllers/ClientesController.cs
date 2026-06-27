using Microsoft.AspNetCore.Mvc;
using Practica2_API.Data;
using Practica2_API.Models;

namespace Practica2_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Cliente>>>> GetAll()
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync();
                return Ok(new ApiResponse<List<Cliente>>(true, "Clientes obtenidos exitosamente", clientes));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<Cliente>>(false, $"Error: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Cliente>>> GetById(long id)
        {
            try
            {
                var cliente = await _clienteRepository.GetByIdAsync(id);
                if (cliente == null)
                    return NotFound(new ApiResponse<Cliente>(false, "Cliente no encontrado"));

                return Ok(new ApiResponse<Cliente>(true, "Cliente obtenido exitosamente", cliente));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Cliente>(false, $"Error: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] ClienteCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(false, "Los datos no son válidos"));

            var (success, idCliente, message) = await _clienteRepository.InsertAsync(request);

            if (!success)
                return BadRequest(new ApiResponse<object>(false, message));

            return CreatedAtAction(nameof(GetById), new { id = idCliente }, 
                new ApiResponse<object>(true, message, new { IdCliente = idCliente }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(long id, [FromBody] ClienteCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(false, "Los datos no son válidos"));

            var (success, message) = await _clienteRepository.UpdateAsync(id, request);

            if (!success)
                return BadRequest(new ApiResponse<object>(false, message));

            return Ok(new ApiResponse<object>(true, message));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(long id)
        {
            var (success, message) = await _clienteRepository.DeleteAsync(id);

            if (!success)
                return BadRequest(new ApiResponse<object>(false, message));

            return Ok(new ApiResponse<object>(true, message));
        }
    }
}
