using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Api.Requests.Category;
using Shop.Api.Interface;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController(ICategoryService _categoryService, IImageService _imageService, IConfiguration _configuration):ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest dto)
    {
        if (dto.Image != null)
        {
            dto.Url = (await _imageService.SaveFileAsync(dto.Image, _configuration["DirnameForFiles:Categories"])) ?? string.Empty;
        }
        var createDto = new CategoryCreateDTO
        {
            Name = dto.Name,
            Url = dto.Url,
            Slug = dto.Slug,
            ParentId = dto.ParentId,
        };
        var id = await _categoryService.CreateCategoryAsync(createDto);
        return Ok($"Category created {id}");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        List<CategoryReadDTO>? categories = await _categoryService.GetAllCategoriesAsync();
        if (categories == null || categories.Count == 0)
        {
            return NotFound();
        }
        return Ok(categories);
    }
}