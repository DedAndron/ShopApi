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
        var product = new Product()
        {
            Title = $"Test Product {id}",
            Price = 100
        };
        return Ok(product);
    }

    [HttpPost]
    public IActionResult AddNewCategory([FromBody] Category category)
    {
        _categoryService.AddCategory(category);
        return Created();
    }
}

