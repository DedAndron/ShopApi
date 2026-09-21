using MediatR;
using Shop.Application.DTOs.ProductDTOs;

namespace Shop.Application.Commands.Products;

public sealed record CreateProductCommand(ProductCreateDTO Product) : IRequest<int?>;