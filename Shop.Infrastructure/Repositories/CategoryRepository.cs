using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository(ShopDbContext _context):ICategoryRepository
{
    public async Task<int?> AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category.Id;

    }

    public async Task<List<Category>?> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return null;
        return new CategoryReadDTO()
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Url = category.Url,
        };
    }
    public async Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO dto)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return null;
        category.Name = dto.Name;
        category.Slug = dto.Slug;
        category.Url = dto.Url;
        await _context.SaveChangesAsync();
        return new CategoryReadDTO()
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Url = category.Url,
        };
    }
    public async Task DeleteCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
