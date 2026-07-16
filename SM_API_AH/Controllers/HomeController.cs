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

        [HttpPost("RecuperarAccesoAPI")]
        public IActionResult RecuperarAccesoAPI(RecuperarAccesoRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);

            var response = context.QueryFirstOrDefault<DatosUsuarioResponseModel>("spValidarCorreo", parameters);

            if (response == null)
                return NotFound("La información no se pudo validar correctamente");

            //Generar nueva contraseña temporal
            var temporal = GenerarContrasena();

            parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", response.Consecutivo);
            parameters.Add("@Contrasenna", temporal);
            parameters.Add("@IndicadorTemp", true);

            var actualizacion = context.Execute("spActualizarContrasenna", parameters);

            if (actualizacion > 0)
            {
                //Enviar un correo electrónico con la nueva contraseña temporal
                return Ok(response);
            }

            return BadRequest("No se ha recuperado su acceso, por favor intente nuevamente.");
        }

        private string GenerarContrasena()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var chars = new char[10];

            for (int i = 0; i < 10; i++)
                chars[i] = caracteres[random.Next(caracteres.Length)];

            return new string(chars);
        }
    }
}