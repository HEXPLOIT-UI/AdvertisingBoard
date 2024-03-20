using Microsoft.AspNetCore.Mvc;

namespace AdvertisingBoard.Services
{
    public interface IAuthorizationService
    {
        Task<IActionResult> RegisterUser(RegisterUserViewModel userDto);

    }
}
