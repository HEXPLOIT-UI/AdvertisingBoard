using AdvertisingBoard.Data;
using AdvertisingBoard.Utils;
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
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthorizationController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] User user)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                {
                    return BadRequest("Email is already registered.");
                } else if (await _context.Users.AnyAsync(u => u.Login == user.Login))
                {
                    return BadRequest("Login is already registered.");
                }
                user.Password = HashUtils.HashPassword(user.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("User registered successfully.");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] string login, [FromForm] string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (HashUtils.VerifyPassword(password, user.Password))
                {
                    var token = GenerateJwtToken(login);
                    return Ok(new { token });
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }
            return BadRequest(ModelState);
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5), // Увеличить на релизе до 48ч.
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
