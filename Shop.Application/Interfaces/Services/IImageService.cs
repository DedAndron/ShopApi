using Microsoft.AspNetCore.Http;

namespace Shop.Api.Interface
{
    public interface IImageService
    {
        Task<string?> SaveProductImageAsync(IFormFile file);
    }
}
