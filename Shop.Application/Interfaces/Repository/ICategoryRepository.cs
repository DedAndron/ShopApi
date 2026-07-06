using Shop.Application.DTOs.CategoryDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<int?> AddCategoryAsync(Category category);
    Task<IEnumerable<CategoryReadDTO>> ListCategoriesAsync();
    Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
}
