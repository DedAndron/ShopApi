using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Services;

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
            return BadRequest("Не вдалося створити замовлення");
        return Ok(new { OrderId = orderId });
    }
}
