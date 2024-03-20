using AdvertisingBoard.Data;
using AdvertisingBoard.ModelsDTO;
using AdvertisingBoard.Services;
using AdvertisingBoard.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        
        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel model)
        {
            return ModelState.IsValid ? Ok(await _authorizationService.RegisterUser(model)) : BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginUserViewModel model)
        {
            return ModelState.IsValid ? Ok(await _authorizationService.LoginUser(model)) : BadRequest(ModelState);
        }
    }

}
