using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Shop.Api.Interface;
using Shop.Api.Middlewares;
using Shop.Api.Services;
using Shop.Application.Interfaces.Helpers;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Infrastructure.Configuration;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Helpers;
using Shop.Infrastructure.Repositories;
using Shop.Infrastructure.Services;
using System.Text;

//DI (Dependency Injection) - реестрация любого класса и внедренние его в любую часть проекта без создания класса.
//Middleware - небольшой компонент кода, который встраивается в конвеер обработки запроса.
//DTO (Data Transfer Object) - простой контейнер для переноса информации между разными частями программы.
//JWT (JSON Web Token) - стандарт для создания токенов доступа, которые позволяют безопасно передавать информацию между сторонами в виде JSON-объектов.
//CORS (Cross-Origin Resource Sharing) - механизм, который позволяет ограничить доступ к ресурсам веб-приложения с других доменов.
//Cache - механизм хранения данных в памяти для ускорения доступа к ним и уменьшения нагрузки на сервер.

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
            builder.Services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection"));
            });

            var configuration = builder.Configuration;
            // ================= JWT Settings =================
            var jwtSettings = configuration
                .GetSection("Jwt")
                .Get<JwtSettings>()
                ?? throw new Exception("JWT settings not configured.");

            //Реєстрація налаштувань в DI, можемо їх читати будь-де
            builder.Services.Configure<JwtSettings>(
                configuration.GetSection("Jwt"));

            // ================= AutoMapper =================
            builder.Services.AddAutoMapper(
                _ => { },
                typeof(CategoryProfile).Assembly,
                typeof(UserProfile).Assembly
            );

            // ================= CORS =================
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("ProductionPolicy", policy =>
            //    {
            //        policy.WithOrigins("https://example.com", "https://www.example.com")
            //              .WithMethods("GET", "POST", "PUT", "DELETE")
            //              .WithHeaders("Content-Type", "Authorization");
            //    });
            //});
            // Add services to the container.
            //DI container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // ================= Swagger + JWT =================
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
            //builder.Services.AddSwaggerGen();
            //-----------------CACHE-------------------
            builder.Services.AddMemoryCache();
            //--------------SERVICES-------------------
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICachingService, MemoryCachingService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddSingleton<IHashHelper, HashHelper>();
            builder.Services.AddScoped<IJWTService, JWTService>();
            //--------------REPOSITORIES
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();


            // ================= Authentication =================
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                //Правила перевірки токена
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)
                    ),

                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestTimerMiddleware>();
            app.UseStaticFiles();
            app.MapControllers();



            app.Run();
        }
    }
}