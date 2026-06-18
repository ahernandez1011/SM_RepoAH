using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SM_API_AH.Models;

namespace SM_API_AH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        [HttpPost("RegistroAPI")]
        public IActionResult RegistroAPI(UsuarioModel model)
        {
                using var context = new SqlConnection("Server=CRRECEPT-1660\\SQLEXPRESS; Database= SM_DB; Integrated Security=True;" +
                    "TrustServerCertificate=True;");

                var parameters = new DynamicParameters();
                parameters.Add("@Identificacion", model.Identificacion);
                parameters.Add("@Nombre", model.Nombre);
                parameters.Add("@CorreoElectronico", model.CorreoElectronico);
                parameters.Add("@Contrasenna", model.Contrasenna);

                var response = context.Query("spRegistrarUsuario", parameters);
                return Ok();
        }
    }
}
