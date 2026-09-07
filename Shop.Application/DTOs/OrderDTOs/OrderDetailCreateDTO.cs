using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.OrderDTOs;

public class OrderDetailCreateDTO
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
