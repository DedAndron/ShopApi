using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Services;

public interface IOrderService
{
    Task<int?> CreateOrderAsync(OrderCreateDTO dto);
}
