using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class AdvertismentController : ControllerBase
    {
        public AdvertismentController()
        {

        }


    }
}
