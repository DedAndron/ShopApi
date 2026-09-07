using System.ComponentModel.DataAnnotations;

namespace Shop.Application.DTOs.OrderDTOs;

/// <summary>
/// Message contract for an order received from the <c>Orders</c> RabbitMQ queue.
/// </summary>
public sealed class QueuedOrderDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<OrderDetailCreateDTO> Details { get; set; } = [];
}