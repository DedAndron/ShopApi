using Microsoft.EntityFrameworkCore;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Data;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Infrastructure.Repositories;

public class OrderRepository(ShopDbContext _context) : IOrderRepository
{
    public async Task<int?> CreateOrderAsync(OrderCreateDTO dto)
    {
        var order = new Order
        {
            CreatedAt = DateTime.UtcNow,
            Details = dto.Details.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity
            }).ToList()
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }
    public async Task<ICollection<Order>?> GetAllOrdersAsync()
    {
        return await _context.Orders.Include(o => o.Details).ToListAsync();
    }
    public async Task<OrderReadDTO?> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders.Include(o => o.Details).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
            return null;
        return new OrderReadDTO()
        {
            Id = order.Id,
            UserId = order.UserId,
            Paid = order.Paid,
            Details = order.Details.Select(od => new OrderDetailReadDTO()
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity
            }).ToList()
        };
    }
    public async Task DeleteOrderByIdAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

}
