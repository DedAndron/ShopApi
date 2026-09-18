using MediatR;
using Shop.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Commands.Product;

public record CreateProductCommand(int id) : IRequest<ProductReadDTO>;
