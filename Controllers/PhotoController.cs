using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("CreatePhotoInAdvertisement")]
        public async Task<IActionResult> CreatePhotoInAdvertisement(List<IFormFile> files, int adId)
        {
            return Ok(await _photoService.CreatePhotoInAdvertisement(files, adId, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }

        [HttpPost("DeletePhotoFromAdvertisement")]
        public async Task<IActionResult> DeletePhotoFromAdvertisement(int photoId)
        {
            return Ok(await _photoService.DeletePhotoFromAdvertisement(photoId, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }

        [HttpGet("GetPhotosFromAdvertisement")]
        public async Task<IActionResult> GetPhotosFromAdvertisement(int adId)
        {
            return Ok(await _photoService.GetPhotosOnAdvertisement(adId));
        }
    }
}
