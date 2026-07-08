namespace Shop.Api.Interface
{
    public interface IImageService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }

}
