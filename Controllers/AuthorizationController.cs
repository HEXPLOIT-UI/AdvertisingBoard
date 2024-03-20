using AdvertisingBoard.Data;
using AdvertisingBoard.ModelsDTO;
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
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthorizationController(ApplicationDbContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserViewModel userDto)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
                {
                    return BadRequest("Email is already registered.");
                } else if (await _context.Users.AnyAsync(u => u.Login == userDto.Login))
                {
                    return BadRequest("Login is already registered.");
                }
                var user = _mapper.Map<User>(userDto);
                user.Password = HashUtils.HashPassword(userDto.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("User registered successfully.");
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (HashUtils.VerifyPassword(model.Password, user.Password))
                {
                    var token = GenerateJwtToken(model.Login, user.Role);
                    return Ok($"Bearer {token}");
                }
                else
                {
                    return Unauthorized("Invalid credentials.");
                }
            }
            return BadRequest(ModelState);
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(48),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
