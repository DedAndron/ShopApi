using Shop.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.OrderDTOs;

public class OrderDetailReadDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
