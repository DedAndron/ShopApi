using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class ProductRepository(ShopDbContext _context) : IProductRepository
{
    public async Task<int?> AddProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product.Id;
    }

    public async Task<List<Product>?> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;
        return new ProductReadDTO()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQty = product.StockQty,
            IsActive = product.IsActive,
        };
    }
    public async Task<ProductReadDTO?> UpdateProductAsync(int id, ProductUpdateDTO dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQty = dto.StockQty;
        product.IsActive = dto.IsActive;
        await _context.SaveChangesAsync();
        return new ProductReadDTO()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQty = product.StockQty,
            IsActive = product.IsActive,
        };
    }
    public async Task DeleteProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
