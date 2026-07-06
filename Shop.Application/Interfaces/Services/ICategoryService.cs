using Shop.Application.DTOs.CategoryDTOs;

namespace Shop.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
    // Отримання всіх категорії
    Task<IEnumerable<CategoryReadDTO>> ListCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
}
