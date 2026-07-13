using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.ProductDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IProductRepository
{
    Task<int?> AddProductAsync(Product product);
    Task<List<Product>?> GetAllProductsAsync();
    Task<ProductReadDTO?> GetProductByIdAsync(int id);
    Task<ProductReadDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto);
    Task DeleteProductByIdAsync(int id);
}
