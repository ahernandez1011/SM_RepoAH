using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API_AH.Models;
using SM_API_AH.Services;

namespace SM_API_AH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactoController(IConfiguration _config, IUtilesService _utiles) : ControllerBase
    {

        [HttpGet("ConsultarSolicitudesChatAPI")]
        public IActionResult ConsultarSolicitudesChatAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@ConsecutivoUsuario", _utiles.ObtenerConsecutivoToken());
            parameters.Add("@ConsecutivoRol", _utiles.ObtenerConsecutivoRolToken());

            var response = context.Query<SolicitudChatResponseModel>("spConsultarSolicitudesAbiertas", parameters);

            if (response.Any())
            {
                return Ok(response);
            }

            return NotFound("No se han encontrado solicitudes abiertas en este momento");
        }
    }
}