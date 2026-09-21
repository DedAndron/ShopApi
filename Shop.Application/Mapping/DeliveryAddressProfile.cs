using AutoMapper;
using Shop.Application.DTOs.DeliveryAddressDTOs;
using ShopDomain.Models;

namespace Shop.Application.Mapping;

public class DeliveryAddressProfile : Profile
{
    public DeliveryAddressProfile()
    {
        CreateMap<DeliveryAddressCreateDTO, DeliveryAddress>();
        CreateMap<DeliveryAddress, DeliveryAddressReadDTO>();
    }
}