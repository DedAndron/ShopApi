using Microsoft.AspNetCore.Mvc;
using Shop.Api.Interface;
using Shop.Api.Requests.Product;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;

namespace Shop.Api.Controllers;
//<summary>
//    Product controller for handling product-related operations.
//</summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService _productService, IConfiguration _configuration) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest dto)
    {
        var createDto = new ProductCreateDTO
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            StockQty = dto.StockQty,
            CategoryId = dto.CategoryId,
        };
        var id = await _productService.CreateProductAsync(createDto);
        return Ok($"Product created {id}");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        List<ProductReadDTO>? products = await _productService.GetAllProductsAsync();
        if (products == null || products.Count == 0)
        {
            return NotFound();
        }
        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        ProductReadDTO? product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDTO dto)
    {
        var product = await _productService.UpdateProductAsync(id, dto);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productService.DeleteProductByIdAsync(id);
        return Ok($"Product with id {id} deleted");
    }
}