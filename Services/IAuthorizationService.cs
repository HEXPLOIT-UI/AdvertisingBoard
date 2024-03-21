using AdvertisingBoard.Repositories;
using AdvertisingBoard.Utils;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdvertisingBoard.Services
{
    public interface IAuthorizationService
    {
        Task<TaskResultViewModel> RegisterUser(RegisterUserViewModel userDto);
        Task<TaskResultViewModel> LoginUser(LoginUserViewModel userDto);

    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthorizationService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<TaskResultViewModel> LoginUser(LoginUserViewModel userDto)
        {
            var user = await _userRepository.GetUserByLoginAsync(userDto.Login);
            if (user == null)
            {
                return new TaskResultViewModel() { State = false, Message = "Пользователь не найден" };
            }

            if (HashUtils.VerifyPassword(userDto.Password, user.Password))
            {
                var token = GenerateJwtToken(userDto.Login, user.Role);
                return new TaskResultViewModel() { State = true, Message = $"Bearer {token}" };
            }
            else
            {
                return new TaskResultViewModel() { State = false, Message = "Неверный пароль" };
            }
        }

        public async Task<TaskResultViewModel> RegisterUser(RegisterUserViewModel userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Password = HashUtils.HashPassword(userDto.Password);
            await _userRepository.AddAsync(user);
            return new TaskResultViewModel() { State = true, Message = "Пользователь создан" };
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
                Expires = DateTime.Now.AddHours(int.TryParse(_configuration["Jwt:Expires"], out int res) ? res : 168),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
