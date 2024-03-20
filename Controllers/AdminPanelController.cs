﻿using AdvertisingBoard.Services;
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

        public AdminPanelController(IAdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
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
    }
}
