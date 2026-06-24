using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService _categoryService) : ControllerBase
{
    [HttpGet]
    public List<Category> GetCategory()
    {
        return _categoryService.GetAllCategories();
    }

    [HttpGet("{id}")]
    public IActionResult GetCategoryById([FromRoute] int id)
    {
        var category = new Category()
        {
            Id = id,
            Title = $"Test Category {id}",
            Description = $"Test Category {id} Description",
            Image = "img",
            createdAt = DateTime.Now,
            updatedAt = DateTime.Now,
            isShow = true
        };
        return Ok(category);
    }

    [HttpPost]
    public IActionResult AddNewCategory([FromBody] Category category)
    {
        _categoryService.AddCategory(category);
        return Created();
    }
}

