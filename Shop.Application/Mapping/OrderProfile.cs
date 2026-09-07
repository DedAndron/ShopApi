using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Shop.Application.DTOs.OrderDTOs;
using ShopDomain.Models;

namespace Shop.Application.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderCreateDTO, Order>();
        
        CreateMap<Order, OrderReadDTO>();
        
    }
}
