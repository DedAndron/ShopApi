using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Requests.Product;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Services;    
using Shop.Application.Queries.Product; // <- add this if GetProductByIdQuery lives here

namespace Shop.Api.Controllers;
//<summary>
//    Product controller for handling product-related operations.
//</summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(
    IProductService _productService, 
    IConfiguration _configuration, 
    IMediator _mediator
    ) : ControllerBase
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
        ICollection<ProductReadDTO>? products = await _productService.GetAllProductsAsync();
        if (products == null || products.Count == 0)
        {
            return NotFound();
        }
        return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        if(result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreateDTO dto)
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