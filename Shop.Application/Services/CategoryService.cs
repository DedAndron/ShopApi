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

    public async Task<List<CategoryReadDTO>?> GetAllCategoriesAsync()
    {
        List<Category>? categories = await _repository.GetAllCategoriesAsync();
        List<CategoryReadDTO> dtos = null;
        if (categories != null && categories.Count > 0)
        {
            dtos = new List<CategoryReadDTO>();
            categories.ForEach(category =>
            {
                dtos.Add(new CategoryReadDTO()
                {
                    Id = category.Id,
                    Slug = category.Slug,
                    Name = category.Name,
                    Url = category.Url,
                    ParentId = category.ParentId
                });
            });
        }
        return dtos;
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