using ShopDomain.Models;
using System.Text;
using System.Text.Json;

namespace Shop.Api.Middlewares
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate _next;
        
        public UserCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST" &&
            context.Request.Path == "/api/user/register")
            {
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                leaveOpen: true);

                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var user = JsonSerializer.Deserialize<User>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

                if (user?.Email == "admin")
                {
                    await _next(context);
                    return;
                }

                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    message = "No authorization"
                });

                return;
            }

            await _next(context);
        
        }
    }
}
