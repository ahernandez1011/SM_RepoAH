using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API_AH.Models;
using SM_API_AH.Services;

namespace SM_API_AH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _config, IUtilesService _utiles) : ControllerBase
    {
        [HttpGet("ConsultarUsuarioAPI")]
        public IActionResult ConsultarUsuarioAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", _utiles.ObtenerConsecutivoToken());

            var response = context.QueryFirstOrDefault<DatosUsuarioResponseModel>("spConsultarUsuario", parameters);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound("El usuario no se pudo encontrar.");
        }

        [HttpPut("CambiarContrasennaAPI")]
        public IActionResult CambiarContrasennaAPI(CambiarAccesoRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", _utiles.ObtenerConsecutivoToken());
            parameters.Add("@Contrasenna", model.Contrasenna);
            parameters.Add("@IndicadorTemp", false);

            var actualizacion = context.Execute("spActualizarContrasenna", parameters);

            if (actualizacion > 0)
            {
                return Ok(actualizacion);
            }

            return BadRequest("La contraseña no se pudo actualizar correctamente.");
        }

        [HttpPut("CambiarPerfilAPI")]
        public IActionResult CambiarPerfilAPI(CambiarPerfilRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", _utiles.ObtenerConsecutivoToken());
            parameters.Add("@Identificacion", model.Identificacion);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);

            var actualizacion = context.Execute("spActualizarPerfil", parameters);

            if (actualizacion > 0)
            {
                return Ok("Sus datos se han actualizado correctamente.");
            }

            return BadRequest("Su información no se pudo actualizar correctamente.");
        }

    }
}