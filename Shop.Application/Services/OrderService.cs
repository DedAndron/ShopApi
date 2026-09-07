using AutoMapper;
using Shop.Application.DTOs.OrderDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Services;

public class OrderService(IOrderRepository _repository, IMapper _mapper, ICachingService _cacheService) : IOrderService
{
    public async Task<int?> CreateOrderAsync(OrderCreateDTO dto)
    {
        var order = _mapper.Map<Order>(dto);
        return await _repository.CreateOrderAsync(dto);
    }
    public async Task<ICollection<OrderReadDTO>?> GetAllOrdersAsync()
    {
        var cache = await _cacheService.GetAsync<ICollection<OrderReadDTO>>("Orders");
        if (cache == null)
        {
            var orders = await _repository.GetAllOrdersAsync();
            cache = _mapper.Map<ICollection<OrderReadDTO>>(orders);
            await _cacheService.SetAsync("Orders", cache, TimeSpan.FromMinutes(3));
        }
        return cache;
    }
    public async Task<OrderReadDTO?> GetOrderByIdAsync(int id)
    {
        var cache = await _cacheService.GetAsync<OrderReadDTO>($"Order:{id}");
        if (cache == null)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            cache = _mapper.Map<OrderReadDTO>(order);
            await _cacheService.SetAsync($"Order:{id}", cache, TimeSpan.FromMinutes(3));
        }
        return cache;
    }
    public async Task DeleteOrderByIdAsync(int id)
    {
        await _repository.DeleteOrderByIdAsync(id);
        await _cacheService.RemoveAsync($"Order:{id}");
    }
}
