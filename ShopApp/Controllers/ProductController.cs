using Microsoft.AspNetCore.Mvc;
using Shop.Api.Interfaces;
using ShopDomain.Models;

namespace Shop.Api.Controllers;
//<summary>
//    Product controller for handling product-related operations.
//</summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    //<summary>
    //    Retrieves a list of all products.
    //</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public List<Product> GetProducts()
    {
        return productService.GetAllProducts();
    }

    //<summary>
    //    Retrieves a product by its ID.
    //</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetProductById([FromRoute] int id)
    {
        var product = productService.GetAllProducts().FirstOrDefault(p => p.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    //<summary>
    //    Adds a new product to the system.
    //</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddNewProduct([FromBody] Product product)
    {
        productService.AddProduct(product);
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
    }
}