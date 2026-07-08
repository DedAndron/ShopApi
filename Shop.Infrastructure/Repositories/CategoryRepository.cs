using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;

namespace Shop.Infrastructure.Repositories;

public class CategoryRepository(ShopDbContext _context):ICategoryRepository
{
    public async Task<int?> AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChangesAsync();
        return category.Id;

    }

    public Task<List<Category>?> GetAllCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
