using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API.Models;

namespace SM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(IConfiguration _config) : ControllerBase
    {

        [HttpPut("CambiarContrasennaAPI")]
        public IActionResult CambiarContrasennaAPI(CambiarAccesoRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", model.Consecutivo);
            parameters.Add("@Contrasenna", model.Contrasenna);
            parameters.Add("@IndicadorTemp", false);

            var actualizacion = context.Execute("spActualizarContrasenna", parameters);

            if (actualizacion > 0)
            {
                //Enviar un correo electrónico con la nueva contraseña temporal
                return Ok(actualizacion);
            }

            return BadRequest("La contraseña no se pudo actualizar correctamente.");
        }

    }
}