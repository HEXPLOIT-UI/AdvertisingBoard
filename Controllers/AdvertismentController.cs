using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class AdvertismentController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public AdvertismentController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetCategory")]
        public async Task<IActionResult> GetCategory([FromForm][Required] string name)
        {
            return Ok(await _categoryService.GetCategory(name));
        }
    }
}
