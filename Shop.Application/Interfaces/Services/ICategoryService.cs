using Shop.Application.DTOs.CategoryDTOs;
using ShopDomain.Models;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
    Task<ICollection<CategoryReadDTO>?> GetAllCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
    Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryCreateDTO dto);
    Task DeleteCategoryByIdAsync(int id);
}
