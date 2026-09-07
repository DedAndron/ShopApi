using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.OrderDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Interfaces.Repository;

public interface IOrderRepository
{
    Task<int?> CreateOrderAsync(OrderCreateDTO dto);
    Task<ICollection<Order>?> GetAllOrdersAsync();
    Task<OrderReadDTO?> GetOrderByIdAsync(int id);
    Task DeleteOrderByIdAsync(int id);
}
