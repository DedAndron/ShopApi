using Microsoft.AspNetCore.Http;
using MediatR;
using Shop.Application.DTOs.ProductDTOs;

namespace Shop.Application.Commands.Products;

public record CreateProductCommand(
    ProductCreateDTO Product,
    IFormFile? Image
) : IRequest<int>;