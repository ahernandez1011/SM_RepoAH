using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok();
        }
    }
}
