using AutoMapper;
using Shop.Application.DTOs.ProductDTOs;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductReadDTO>()
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(
                    src => src.Images
                        .Select(x => x.FileName)
                        .FirstOrDefault()
        )
    );
    }
}
