using Microsoft.AspNetCore.Mvc;
using Shop.Api.Filters;
using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Controllers
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
