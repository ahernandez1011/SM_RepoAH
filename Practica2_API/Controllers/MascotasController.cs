using Microsoft.AspNetCore.Mvc;
using Practica2_API.Data;
using Practica2_API.Models;

namespace Practica2_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotasController : ControllerBase
    {
        private readonly IMascotaRepository _mascotaRepository;

        public MascotasController(IMascotaRepository mascotaRepository)
        {
            _mascotaRepository = mascotaRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<Mascota>>>> GetAll()
        {
            try
            {
                var mascotas = await _mascotaRepository.GetAllAsync();
                return Ok(new ApiResponse<List<Mascota>>(true, "Mascotas obtenidas exitosamente", mascotas));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<Mascota>>(false, $"Error: {ex.Message}"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Mascota>>> GetById(long id)
        {
            try
            {
                var mascota = await _mascotaRepository.GetByIdAsync(id);
                if (mascota == null)
                    return NotFound(new ApiResponse<Mascota>(false, "Mascota no encontrada"));

                return Ok(new ApiResponse<Mascota>(true, "Mascota obtenida exitosamente", mascota));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<Mascota>(false, $"Error: {ex.Message}"));
            }
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<ApiResponse<List<Mascota>>>> GetByCliente(long idCliente)
        {
            try
            {
                var mascotas = await _mascotaRepository.GetByClienteAsync(idCliente);
                return Ok(new ApiResponse<List<Mascota>>(true, "Mascotas obtenidas exitosamente", mascotas));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<List<Mascota>>(false, $"Error: {ex.Message}"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> Create([FromBody] MascotaCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(false, "Los datos no son válidos"));

            var (success, idMascota, message) = await _mascotaRepository.InsertAsync(request);

            if (!success)
                return BadRequest(new ApiResponse<object>(false, message));

            return CreatedAtAction(nameof(GetById), new { id = idMascota }, 
                new ApiResponse<object>(true, message, new { IdMascota = idMascota }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(long id, [FromBody] MascotaCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(false, "Los datos no son válidos"));

            var (success, message) = await _mascotaRepository.UpdateAsync(id, request);

            if (!success)
                return BadRequest(new ApiResponse<object>(false, message));

            return Ok(new ApiResponse<object>(true, message));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(long id)
        {
            var (success, message) = await _mascotaRepository.DeleteAsync(id);

            if (!success)
                return BadRequest(new ApiResponse<object>(false, message));

            return Ok(new ApiResponse<object>(true, message));
        }
    }
}
