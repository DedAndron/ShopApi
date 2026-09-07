using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Shop.Application.DTOs.OrderDTOs;
using ShopDomain.Models;


namespace Shop.Application.Mapping;

public class OrderDetailProfile : Profile
{
    public OrderDetailProfile()
    {
        CreateMap<OrderDetailCreateDTO, OrderDetail>();
        CreateMap<OrderDetail, OrderDetailReadDTO>();
    }
    
}
