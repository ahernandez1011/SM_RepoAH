using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SM_API_AH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        [HttpPost("RegistroAPI")]
        public IActionResult RegistroAPI()
        {
            return Ok();
        }
    }
}
