using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs.CategoryDTOs;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController(ICategoryService _categoryService):ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO dto)
    {
        int? id = await _categoryService.CreateCategoryAsync(dto);
        return Ok($"Category created {id}");
    }
}