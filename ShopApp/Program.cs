
using ShopApp.Interfaces;
using ShopApp.Middlewares;
using ShopApp.Services;

//DI (Dependency Injection) - реестрация любого класса и внедренние его в любую часть проекта без создания класса.
//Middleware - небольшой компонент кода, который встраивается в конвеер обработки запроса.
//DTO (Data Transfer Object) - простой контейнер для переноса информации между разными частями программы.

namespace ShopApp
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimerMiddleware>();
        }
        public static IApplicationBuilder UseUserChecker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserCheckMiddleware>();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.MapOpenApi();
            //}

            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseRequestTimer();
            app.UseUserChecker();
            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
