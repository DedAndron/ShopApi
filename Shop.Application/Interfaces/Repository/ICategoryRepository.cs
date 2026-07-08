using Shop.Application.DTOs.CategoryDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<int?> AddCategoryAsync(Category category);
    Task<List<Category>?> GetAllCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
}
