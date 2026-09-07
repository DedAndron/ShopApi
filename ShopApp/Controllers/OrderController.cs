using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;
using System.Security.Claims;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController(IOrderService _orderService, IQueueService _queueService, IHttpContextAccessor _httpContextAccessor) : ControllerBase
{
    private const string OrdersQueue = "Orders";

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDTO dto)
    {
        if (dto.Details.Any(detail => detail.ProductId <= 0 || detail.Quantity <= 0))
            return BadRequest("Every order item must contain a valid product ID and a positive quantity.");
        var email = _httpContextAccessor.HttpContext?
            .User
            .FindFirst(ClaimTypes.Email)?
            .Value;

        if (string.IsNullOrEmpty(email))
            return null;


        var queuedOrder = new QueuedOrderDTO
        {
            Email = email,
            Details = dto.Details
        };

        await _queueService.PublishAsync(OrdersQueue, queuedOrder);

        return Accepted(new { Message = "Order has been queued for processing." });
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