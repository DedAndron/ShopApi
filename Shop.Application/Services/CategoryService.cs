using System.Linq;
using AutoMapper;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;

namespace Shop.Application.Services;

public class CategoryService(ICategoryRepository _repository,IMapper _mapper) : ICategoryService
{
    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        return await _repository.AddCategoryAsync(category);
    }

    public async Task<List<CategoryReadDTO>?> GetAllCategoriesAsync()
    {
        List<Category>? categories = await _repository.GetAllCategoriesAsync();
        List<CategoryReadDTO>? dtos = null;
        if (categories != null && categories.Count > 0)
        {
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);
        }
        return dtos;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var category = await _repository.GetCategoryByIdAsync(id);
        if (category == null)
            return null;
        return _mapper.Map<CategoryReadDTO>(category);
    }
    public async Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryUpdateDTO dto)
    {
        var category = await _repository.UpdateCategoryAsync(id, dto);
        if (category == null)
            return null;
        return _mapper.Map<CategoryReadDTO>(category);
    }
    public async Task DeleteCategoryByIdAsync(int id)
    {
        await _repository.DeleteCategoryByIdAsync(id);
    }
}