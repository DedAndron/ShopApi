using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.DeliveryAddressDTOs;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class AuthController(IAuthService _authService, IQueueService _queueService, ShopDbContext _dbContext, IJWTService _jwtService) : ControllerBase
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
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(providerId))
            return BadRequest("Зовнішній провайдер не повернув email або ідентифікатор користувача.");

        const string providerName = "google";
        var provider = await _dbContext.Providers.SingleAsync(provider => provider.Name == providerName);
        var user = await _dbContext.Users.SingleOrDefaultAsync(user => user.Email == email);

        if (user is null)
        {
            user = new User
            {
                Email = email,
                PasswordHash = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)),
                IsVerified = true
            };
            _dbContext.Users.Add(user);
        }

        var linkedProvider = await _dbContext.UserProviders.SingleOrDefaultAsync(link =>
            link.ProviderId == provider.Id && link.NumberProvider == providerId);

        if (linkedProvider is not null && linkedProvider.UserId != user.Id)
            return Conflict("Цей обліковий запис зовнішнього провайдера вже прив'язаний до іншого користувача.");

        var userProvider = await _dbContext.UserProviders.SingleOrDefaultAsync(link =>
            link.UserId == user.Id && link.ProviderId == provider.Id);

        if (userProvider is not null && userProvider.NumberProvider != providerId)
            return Conflict("Користувач вже має інший обліковий запис цього провайдера.");

        if (linkedProvider is null && userProvider is null)
        {
            _dbContext.UserProviders.Add(new UserProvider
            {
                User = user,
                ProviderId = provider.Id,
                NumberProvider = providerId
            });
        }

        var (refreshTokenValue, expiresInDays) = _jwtService.GenerateRefreshToken();
        _dbContext.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(expiresInDays),
            User = user
        });

        await _dbContext.SaveChangesAsync();

        var accessToken = _jwtService.GenerateAccessToken(user.Email, user.Role.ToString());

        Response.Cookies.Append("refreshToken", refreshTokenValue, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(expiresInDays)
        });

        return Ok(new { Name = name, Email = user.Email, ProviderId = providerId, AccessToken = accessToken });

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