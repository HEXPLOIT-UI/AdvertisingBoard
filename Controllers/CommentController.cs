using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Advertisment.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, User")]
public class CommentController : ControllerBase
{
    public readonly ICommentService _commentService;
    public CommentController(ICommentService commentService)
    {
        _commentService = commentService; 
    }

    [HttpPost("CreateComment")]
    public async Task<IActionResult> CreateComment([FromForm] CommentViewModel model)
    {
        return Ok(await _commentService.CreateComment(model, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
    }
    
    [HttpPost("DeleteComment")]
    public async Task<IActionResult> DeleteComment([Required]int id)
    {
        return Ok(await _commentService.DeleteComment(id, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
    }
    
    [HttpPost("UpdateComment")]
    public async Task<IActionResult> UpdateComment([FromForm] CommentViewModel model, [Required]int id)
    {
        return Ok(await _commentService.UpdateComment(id, model, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
    }
    [HttpPost("GetCommentsByAdvertisment")]
    public async Task<IActionResult> GetCommentsByAdId([Required] int adId)
    {
        return Ok(await _commentService.GetCommentsByAdId(adId));
    }

}