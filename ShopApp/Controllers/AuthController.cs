using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Services;
using System.Security.Claims;
using Shop.Application.DTOs.DeliveryAddressDTOs;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class AuthController(IAuthService _authService, IQueueService _queueService) : ControllerBase
{
    // Вхід через Google
    [HttpGet("login-google")]
    public IActionResult LoginGoogle()

    {

        var properties = new AuthenticationProperties
        {

            RedirectUri = Url.Action(nameof(ExternalResponse))

        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);

    }
    // Зворотний виклик після успішної авторизації
    [HttpGet("external-response")]
    public async Task<IActionResult> ExternalResponse()

    {

        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);


        if (!result.Succeeded)

            return BadRequest("Помилка зовнішньої аутентифікації.");


        var claims = result.Principal.Identities.FirstOrDefault()?.Claims;


        var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var providerId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


        // Тут зазвичай виконується:
        // 1. Пошук користувача в БД за email/providerId.
        // 2. Реєстрація нового користувача, якщо його немає.
        // 3. Генерація власного JWT (якщо це SPA / Mobile) або встановлення локальної сесії.

        return Ok(new { Name = name, Email = email, ProviderId = providerId });

    }
    [HttpPost("logout")]

    [Authorize]

    public async Task<IActionResult> Logout()

    {

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok("Вихід успішний.");
    }   
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO dto, CancellationToken cancellationToken)
    {
        var user = await _authService.RegisterAsync(dto);
        if (user.User == null || user.Token == null)
           return BadRequest("Користувач за таким email вже існує");
        await _queueService.PublishAsync("Users", dto);
        Response.Cookies.Append("refreshToken", user.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            //Expires = new DateTimeOffset(dbDate);
        });
        var response = new { user = user.User, token = user.Token };
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO dto)
    {
        //TODO: Зробити роут для входа
        return Ok();
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(new
        {
            Email = email,
            Role = role
        });
    }
    [Authorize]
    [HttpPost("addresses")]
    public async Task<IActionResult> AddDeliveryAddress(
        [FromBody] DeliveryAddressCreateDTO dto,
        CancellationToken cancellationToken)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return Unauthorized();

        var address = await _authService.AddDeliveryAddressAsync(email, dto, cancellationToken);
        if (address is null)
            return NotFound("Користувача не знайдено");

        return CreatedAtAction(nameof(Profile), new { }, address);
    }
    [Authorize(Roles = "Admin")]
    [HttpPut("{userId:guid}/role")]
    public async Task<IActionResult> ChangeUserRole(string email, [FromBody] UserChangeRoleDTO dto)
    {
        var user = await _authService.ChangeUserRoleAsync(email, dto);
        if (user == null)
            return NotFound("Користувача не знайдено");

        return Ok(user);
    }
}