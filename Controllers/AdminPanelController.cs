using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IAdminPanelService _adminPanelService;
        private readonly IAdvertisementService _advertisementService;

        public AdminPanelController(IAdminPanelService adminPanelService, IAdvertisementService advertisementService)
        {
            _adminPanelService = adminPanelService;
            _advertisementService = advertisementService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Доступ разрешён!");
        }

        [HttpPost("SetAdmin")]
        public async Task<IActionResult> SetAdmin([FromForm][Required] string login, [FromForm][Required] bool isAdmin)
        {
            return Ok(await _adminPanelService.SetAdmin(login, isAdmin));
        }

        [HttpPost("DeleteAllAdvertisements")]
        public async Task<IActionResult> DeleteAllAdvertisements()
        {
            return Ok(await _advertisementService.DeleteAllAdvertisements());
        }
    }
}
