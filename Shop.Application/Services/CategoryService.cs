using System.Linq;
using AutoMapper;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;

namespace Shop.Application.Services;

public class CategoryService(ICategoryRepository _repository,IMapper _mapper,ICachingService _cacheService) : ICategoryService
{
    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        return await _repository.AddCategoryAsync(category);
    }

    public async Task<ICollection<CategoryReadDTO>?> GetAllCategoriesAsync()
    {
        var cache = await _cacheService.GetAsync<ICollection<CategoryReadDTO>>("Categories");
        if (cache == null)
        {
            var categories = await _repository.GetAllCategoriesAsync();
            cache = _mapper.Map<ICollection<CategoryReadDTO>>(categories);
            await _cacheService.SetAsync("Categories", cache, TimeSpan.FromMinutes(3));
        }
        return cache;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var cache = await _cacheService.GetAsync<CategoryReadDTO>("Categories");
        if (cache == null)
        {
            var categories = await _repository.GetCategoryByIdAsync(id);
            cache = _mapper.Map<CategoryReadDTO>(categories);
            //await _cacheService.SetAsync("Categories", cache, TimeSpan.FromMinutes(3));
        }
        return cache;
    }
    public async Task<CategoryReadDTO?> UpdateCategoryAsync(int id, CategoryCreateDTO dto)
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