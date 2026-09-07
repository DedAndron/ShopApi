using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.OrderDTOs;

public class OrderReadDTO
{
    public int Id { get; set; }
    public Guid? UserId { get; set; }
    public bool Paid { get; set; }
    public List<OrderDetailReadDTO> Details { get; set; } = new();
}
