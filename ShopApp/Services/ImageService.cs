using Shop.Api.Interface;

namespace Shop.Api.Services
{
    public class ImageService(IWebHostEnvironment environment) : IImageService
    {
        public async Task<string?> SaveProductImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(
                environment.WebRootPath,
                "products"
            );

            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(
                filePath,
                FileMode.Create
            );

            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
