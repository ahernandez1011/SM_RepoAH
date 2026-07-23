using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API_AH.Models;
using SM_API_AH.Services;

namespace SM_API_AH.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IConfiguration _config, IUtilesService _utiles) : ControllerBase
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

            if (response > 0)
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

            if (response != null && BCrypt.Net.BCrypt.Verify(model.Contrasenna, response.Contrasenna))
            {
                response.Token = _utiles.GenerarToken(response.Consecutivo);
                return Ok(response);
            }
            return NotFound("La información no se pudo validar correctamente.");
        }

        [HttpPost("RecuperarAccesoAPI")]
        public async Task<IActionResult> RecuperarAccesoAPI(RecuperarAccesoRequestModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@CorreoElectronico", model.CorreoElectronico);

            var response = context.QueryFirstOrDefault<DatosUsuarioResponseModel>("spValidarCorreo", parameters);

            if (response == null)
                return NotFound("La información no se pudo validar correctamente.");

            //Generar nueva contraseña temporal
            var temporal = _utiles.GenerarContrasena();
            var temporalHash = BCrypt.Net.BCrypt.HashPassword(temporal);

            parameters = new DynamicParameters();
            parameters.Add("@Consecutivo", response.Consecutivo);
            parameters.Add("@Contrasenna", temporalHash);
            parameters.Add("@IndicadorTemp", true);

            var actualizacion = context.Execute("spActualizarContrasenna", parameters);

            if (actualizacion > 0)
            {
                //Enviar un correo electrónico con la nueva contraseña temporal
                string ruta = Path.Combine(AppContext.BaseDirectory, "Templates", "RecuperarAcceso.html");
                string plantilla = System.IO.File.ReadAllText(ruta);

                plantilla = plantilla.Replace("{{NOMBRE}}", response.Nombre);
                plantilla = plantilla.Replace("{{TEMPORAL}}", temporal);

                await _utiles.EnviarCorreoAsync(response.CorreoElectronico, "Recuperación de acceso", plantilla);
                return Ok(response);
            }

            return BadRequest("No se ha recuperado su acceso, por favor intente nuevamente.");
        }
    }
}