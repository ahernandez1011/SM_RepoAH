using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API_AH.Models;

namespace SM_API_AH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IConfiguration _config) : ControllerBase
    {
        [HttpPost("RegistroAPI")]
        public IActionResult RegistroAPI(RegistroUsuarioRequestModel model)
        {
                using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

                var parameters = new DynamicParameters();
                parameters.Add("@Identificacion", model.Identificacion);
                parameters.Add("@Nombre", model.Nombre);
                parameters.Add("@CorreoElectronico", model.CorreoElectronico);
                parameters.Add("@Contrasenna", model.Contrasenna);

                var response = context.Execute("spRegistrarUsuario", parameters);

                if(response > 0)
                   return Ok(response);

                return BadRequest("La información no se pudo registrar correctamente.");
        }


        [HttpPost("IniciarSesionAPI")]
        public IActionResult IniciarSesionAPI(InicioSesionRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            parameters.Add("@Contrasenna", model.Contrasenna);

            var response = context.QueryFirstOrDefault<DatosUsuarioResponseModel>("spIniciarSesionUsuario", parameters);

            if(response != null)
                return Ok(response);

            return NotFound("La información no se pudo validar correctamente.");
        }
    }  
}
