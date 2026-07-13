using System.Linq;
using AutoMapper;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;

namespace Shop.Application.Services;

public class ProductService(IProductRepository _repository,IMapper _mapper) : IProductService
{
    public async Task<int?> CreateProductAsync(ProductCreateDTO dto)
    {
        var product = _mapper.Map<Product>(dto);
        return await _repository.AddProductAsync(product);
    }

    public async Task<List<ProductReadDTO>?> GetAllProductsAsync()
    {
        List<Product>? products = await _repository.GetAllProductsAsync();
        List<ProductReadDTO>? dtos = null;
        if (products != null && products.Count > 0)
        {
            dtos = _mapper.Map<List<ProductReadDTO>>(products);
        }
        return dtos;
    }

    public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null)
            return null;
        return _mapper.Map<ProductReadDTO>(product);
    }
    public async Task<ProductReadDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto)
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
