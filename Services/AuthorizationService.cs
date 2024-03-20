using Microsoft.AspNetCore.Mvc;

namespace AdvertisingBoard.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        public AuthorizationService()
        {

        }

        public Task<IActionResult> RegisterUser(RegisterUserViewModel userDto)
        {
            throw new NotImplementedException();
        }
    }
}
