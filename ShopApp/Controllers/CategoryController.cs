using Microsoft.AspNetCore.Mvc;
using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public List<Category> GetCategory()
    {
        return categoryService.GetAllCategories();
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCategoryById([FromRoute] int id)
    {
        var category = categoryService.GetAllCategories().FirstOrDefault(c => c.Id == id);

        if (category is null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public IActionResult AddNewCategory([FromBody] Category category)
    {
        categoryService.AddCategory(category);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }
}