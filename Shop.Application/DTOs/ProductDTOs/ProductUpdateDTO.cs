using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.DTOs.ProductDTOs;

public class ProductUpdateDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQty { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}
