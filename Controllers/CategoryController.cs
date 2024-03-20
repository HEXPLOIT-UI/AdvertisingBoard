using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("CreateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryViewModel model)
        {
            return Ok(await _categoryService.CreateCategory(model));
        }

        [HttpPost("UpdateCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory([Required] int id, [FromForm] CategoryViewModel model)
        {
            return Ok(await _categoryService.ModifyCategory(id, model));
        }

        [HttpPost("DeleteCategory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory([Required] int id)
        {
            return Ok(await _categoryService.DeleteCategory(id));
        }

        [HttpGet("GetChildrenCategories")]
        public async Task<IEnumerable<Category>> GetChildrenCategories([Required] int id)
        {
            return await _categoryService.GetCategoriesByParentId(id);
        }

        [HttpGet("GetParentsCategories")]
        public async Task<IEnumerable<Category>> GetParentsCategories()
        {
            return await _categoryService.GetAllParentsCategories();
        }
    }
}
