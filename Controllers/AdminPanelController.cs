using AdvertisingBoard.ModelsDTO;
using AdvertisingBoard.Repositories;
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
        private readonly ICategoryService _categoryService;
        public AdminPanelController(IAdminPanelService adminPanelService, ICategoryService categoryService)
        {
            _adminPanelService = adminPanelService;
            _categoryService = categoryService;
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

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryViewModel model)
        {
            return ModelState.IsValid ? Ok(await _categoryService.CreateCategory(model)) : BadRequest(ModelState);
        }

        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromForm][Required] string name)
        {
            return Ok(await _categoryService.DeleteCategory(name));
        }

        [HttpPost("ModifyCategory")]
        public async Task<IActionResult> ModifyCategory([FromForm] CategoryViewModel model)
        {
            return ModelState.IsValid ? Ok(await _categoryService.ModifyCategory(model)) : BadRequest(ModelState);
        }
    }
}
