using Shop.Application.DTOs.ProductDTOs;

namespace Shop.Application.Interfaces.Services;

public interface IProductService
{
    Task<int?> CreateProductAsync(ProductCreateDTO dto);
    Task<List<ProductReadDTO>?> GetAllProductsAsync();
    Task<ProductReadDTO?> GetProductByIdAsync(int id);
    Task<ProductReadDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto);
    Task DeleteProductByIdAsync(int id);
}
