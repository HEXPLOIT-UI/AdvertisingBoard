﻿using AdvertisingBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpPost("CreateAdvertisement")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> CreateAdvertisement([FromForm] AdvertisementViewModel model)
        {
            return Ok(await _advertisementService.CreateAdvertisement(model, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }

        [HttpPost("DeleteAdvertisement")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeleteAdvertisement([Required] int id)
        {
            return Ok(await _advertisementService.DeleteAdvertisement(id, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }
        
        [HttpPost("UpdateAdvertisement")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateAdvertisement([Required] int id, [FromForm] AdvertisementViewModel model)
        {
            return Ok(await _advertisementService.UpdateAdvertisement(id, model, HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value));
        }

        [HttpGet("GetAdvertisement")]
        public async Task<IActionResult> GetAdvertisement([Required] int id)
        {
            return Ok(await _advertisementService.GetAdvertisementById(id));
        }

        [HttpGet("GetAllAdvertisements")]
        public async Task<IActionResult> GetAllAdvertisements(int page, int categoryId = -1)
        {
            return Ok(await _advertisementService.GetAllAdvertisementsOnPage(page, categoryId));
        }
        
        [HttpGet("SearchAdvertisements")]
        public async Task<IActionResult> SearchAdvertisements(int page, string keyword, int categoryId = -1, string contacts = "")
        {
            return Ok(await _advertisementService.SearchAdvertisementByKeywordsOnPage(page, keyword, categoryId, contacts));
        }
    }
}
