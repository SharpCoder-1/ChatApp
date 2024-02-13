using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class PlayController : ControllerBase
    {
        [HttpGet("get-players")]
        public IActionResult Players()
        {
            return Ok(new JsonResult(new{message = "Only authorized viewers can see this"}));
        }
    }
}
