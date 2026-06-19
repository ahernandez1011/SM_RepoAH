using Dapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SM_API_AH.Controllers
{
    [ApiExplorerSettings(IgnoreApi =true)]
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController (IConfiguration _config) : ControllerBase
    {
        [Route("RegistrarError")]
        public IActionResult RegistrarError()    
        {
            var exception= HttpContext.Features.Get<IExceptionHandlerFeature>();

            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Mensaje", exception?.Error.Message);
            parameters.Add("@Lugar", exception?.Path);
            parameters.Add("@FechaHora", DateTime.Now);
            parameters.Add("@ConsecutivoUsuario", 0);

            context.Execute("spRegistrarError", parameters);
            return StatusCode(500, "Se presentó un inconveniente técnico.");
        }
    }
}
