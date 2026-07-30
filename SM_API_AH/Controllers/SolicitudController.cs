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
    public class SolicitudController(IConfiguration _config, IUtilesService _utiles) : ControllerBase
    {

        [HttpPost("RegistrarSolicitudAPI")]
        public IActionResult RegistrarSolicitudAPI(RegistroSolicitudRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Titulo", model.Titulo);
            parameters.Add("@Descripcion", model.Descripcion);
            parameters.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());

            var response = context.Execute("spRegistrarSolicitud", parameters);

            if (response > 0)
            {
                return Ok("La solicitud se ha registrado correctamente");
            }

            return BadRequest("No se ha podido registrar la solicitud");
        }

        [HttpGet("ConsultarSolicitudesUsuarioAPI")]
        public IActionResult ConsultarSolicitudesUsuarioAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());

            var response = context.Query<SolicitudResponseModel>("spConsultarSolicitudesUsuario", parameters);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound("No se encontraron solicitudes");
        }


        [HttpGet("ConsultarSolicitudAPI")]
        public IActionResult ConsultarSolicitudAPI(int consecutivoSolicitud)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@ConsecutivoSolicitud", consecutivoSolicitud);

            var response = context.QueryFirstOrDefault<SolicitudResponseModel>("spConsultarSolicitud", parameters);

            if (response != null)
            {
                return Ok(response);
            }

            return NotFound("No se ha encontrado la solicitud");
        }


        [HttpDelete("CancelarSolicitudAPI")]
        public IActionResult CancelarSolicitudAPI(int consecutivoSolicitud)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@ConsecutivoSolicitud", consecutivoSolicitud);
            parameters.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());

            var response = context.Execute("spCancelarSolicitudUsuario", parameters);

            if (response > 0)
            {
                return Ok("La solicitud se ha cancelado correctamente");
            }

            return BadRequest("No se ha podido registrar la solicitud");
        }

    }
}
