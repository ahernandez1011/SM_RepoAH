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

                context.Execute("spRegistrarUsuario", parameters);
                return Ok();
        }


        [HttpPost("IniciarSesionAPI")]
        public IActionResult IniciarSesionAPI(InicioSesionRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);
            parameters.Add("@Contrasenna", model.Contrasenna);

            var response = context.QueryFirstOrDefault<DatosUsuarioResponseModel>("spIniciarSesionUsurio", parameters);

            if(response != null)
                return Ok(response);

            return NotFound("La información no se pudo validar correctamente.");
        }
    }  
}
