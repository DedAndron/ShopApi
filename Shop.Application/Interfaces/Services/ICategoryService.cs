using Shop.Application.DTOs.CategoryDTOs;
using ShopDomain.Models;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
    Task<List<CategoryReadDTO>?> GetAllCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
}
