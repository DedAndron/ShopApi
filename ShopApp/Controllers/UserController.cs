using Microsoft.AspNetCore.Mvc;
using ShopApp.Filters;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService _userService):ControllerBase
    {
        [HttpPost("register")]
        [UserActionFilter]
        public IActionResult RegisterUser([FromBody] User user)
        {
            _userService.RegisterUser(user);
            return Created();
        }
    }
}
