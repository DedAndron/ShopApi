using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Application.Services;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController(IOrderService _orderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO dto)
    {
        var orderId = await _orderService.CreateOrderAsync(dto);
        if (orderId == null)
            return BadRequest("Failed to create order");
        return Ok(new { OrderId = orderId });
    }
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        if (orders == null)
            return NotFound();
        return Ok(orders);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
            return NotFound();
        return Ok(order);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _orderService.DeleteOrderByIdAsync(id);
        return Ok($"Order with id {id} deleted");
    }
}
