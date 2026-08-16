using AutoMapper;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System.Linq;

namespace Shop.Application.Services;

public class ProductService(IProductRepository _repository,IMapper _mapper,ICachingService _cacheService) : IProductService
{
    public async Task<int?> CreateProductAsync(ProductCreateDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        return await _repository.AddProductAsync(product);
    }

    public async Task<ICollection<ProductReadDTO>?> GetAllProductsAsync()
    {
        var cache = await _cacheService.GetAsync<ICollection<ProductReadDTO>>("Products");
        if (cache == null)
        {
            var products = await _repository.GetAllProductsAsync();
            cache = _mapper.Map<ICollection<ProductReadDTO>>(products);
            await _cacheService.SetAsync("Products", cache, TimeSpan.FromMinutes(3));
        }
        return cache;
    }

    public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null)
            return null;
        return _mapper.Map<ProductReadDTO>(product);
    }
    public async Task<ProductReadDTO?> UpdateProductAsync(int id, ProductCreateDTO dto)
    {
        var product = await _repository.UpdateProductAsync(id, dto);
        if (product == null)
            return null;
        return _mapper.Map<ProductReadDTO>(product);
    }
    public async Task DeleteProductByIdAsync(int id)
    {
        await _repository.DeleteProductByIdAsync(id);
    }
}
