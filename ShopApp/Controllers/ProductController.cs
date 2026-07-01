using Microsoft.AspNetCore.Mvc;
using Shop.Api.Interfaces;
using ShopDomain.Models;
namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService _productService):ControllerBase
{
    //private readonly IProductService _productService;
    //public ProductController(IProductService productService)
    //{
    //    _productService = productService;
    //}
    [HttpGet]
    public List<Product> GetProducts()
    {
        return _productService.GetAllProducts();
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById([FromRoute] int id)
    {
        var product = new Product()
        {
            Name = $"Test Product {id}",
            Price = 100
        };
        return Ok(product);
    }

    [HttpPost]
    public IActionResult AddNewProduct([FromBody] Product product)
    {
        _productService.AddProduct(product);
        return Created();
    }
}
