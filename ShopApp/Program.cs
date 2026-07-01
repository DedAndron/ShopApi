
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Shop.Api.Interfaces;
using Shop.Api.Middlewares;
using Shop.Api.Services;
using Shop.Infrastructure.Data;
using System.Reflection;

//DI (Dependency Injection) - реестрация любого класса и внедренние его в любую часть проекта без создания класса.
//Middleware - небольшой компонент кода, который встраивается в конвеер обработки запроса.
//DTO (Data Transfer Object) - простой контейнер для переноса информации между разными частями программы.

namespace Shop.Api
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
            builder.Services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Products API",
                    Version = "v1",
                    Description = "API for work with products"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
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