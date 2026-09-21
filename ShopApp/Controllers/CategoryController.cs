using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Interface;
using Shop.Api.Requests.Category;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Commands.Categories;
using Shop.Application.Interfaces.Services;
using Shop.Application.Queries.Category;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController(IMediator _mediator,
    ICategoryService _categoryService,
    IImageService _imageService,
    IConfiguration _configuration) : ControllerBase
{

    [HttpPost("create")]
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
        var id = await _mediator.Send(new CreateCategoryCommand(createDto));
        return Ok($"Category created {id}");
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllCategories()
    {

        ICollection<CategoryReadDTO>? categories = await _categoryService.GetAllCategoriesAsync();
        if (categories == null || categories.Count == 0)
        {
            return NotFound();
        }
        return Ok(categories);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpGet("{slug}")]

    public async Task<ActionResult<CategoryReadDTO>> GetCategoryBySlug(string slug)
    {
        var result = await _mediator.Send(new GetCategoryBySlugQuery(slug));
        if (result == null)
            return NotFound();

        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCategory(int id, [FromForm] CategoryCreateDTO dto)
    {
        var category = await _categoryService.UpdateCategoryAsync(id, dto);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategoryByIdAsync(id);
        return Ok($"Category with id {id} deleted");
    }
}