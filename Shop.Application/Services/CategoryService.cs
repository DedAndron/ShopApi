using System.Linq;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;

namespace Shop.Application.Services;

public class CategoryService(ICategoryRepository _repository) : ICategoryService
{
    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        return await _repository.AddCategoryAsync(new Category()
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Url = dto.Url,
        });
    }

    public async Task<IEnumerable<CategoryReadDTO>> ListCategoriesAsync()
    {
        var categories = await _repository.ListCategoriesAsync();
        return categories.Select(c => new CategoryReadDTO()
        {
            Id = c.Id,
            Name = c.Name,
            Slug = c.Slug,
            Url = c.Url,
        }).ToList();
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var category = await _repository.GetCategoryByIdAsync(id);
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
}