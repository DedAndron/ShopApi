using Shop.Application.DTOs.ProductDTOs;
namespace Shop.Api.Requests.Product
{
    public class ProductCreateRequest : ProductCreateDTO
    {
        public IFormFile? Image { get; set; }
    }
}
