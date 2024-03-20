using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : ControllerBase
    {
        public AdminPanelController()
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Доступ разрешён!");
        }

        [HttpPost("SetAdmin")]
        public async Task<IActionResult> SetAdmin([FromForm] string login)
        {
            return Ok(login);
        }
    }
}
