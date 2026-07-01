using Microsoft.AspNetCore.Mvc;
using Shop.Api.Filters;
using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        [UserActionFilter]
        public IActionResult RegisterUser([FromBody] User user)
        {
            userService.RegisterUser(user);
            return CreatedAtAction(nameof(RegisterUser), new { id = user.Id }, user);
        }
    }
}