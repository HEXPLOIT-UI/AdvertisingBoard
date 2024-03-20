using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpPost("CreateAdvertisement")]
        public async Task<IActionResult> CreateAdvertisement([FromForm] AdvertisementViewModel model)
        {
            return Ok(await _advertisementService.CreateAdvertisement(model, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }

        [HttpPost("DeleteAdvertisement")]
        public async Task<IActionResult> DeleteAdvertisement([Required] int id)
        {
            return Ok(await _advertisementService.DeleteAdvertisement(id, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }
        
        [HttpPost("UpdateAdvertisement")]
        public async Task<IActionResult> UpdateAdvertisement([Required] int id, [FromForm] AdvertisementViewModel model)
        {
            return Ok(await _advertisementService.UpdateAdvertisement(id, model, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }

        [HttpGet("GetAdvertisement")]
        public async Task<IActionResult> GetAdvertisement([Required] int id)
        {
            return Ok(await _advertisementService.GetAdvertisementById(id));
        }
    }
}
