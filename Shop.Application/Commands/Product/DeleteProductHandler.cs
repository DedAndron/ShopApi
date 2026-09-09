using MediatR;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Commands.Product;

public class DeleteProductHandler(IProductRepository _repository) : IRequestHandler<DeleteProductCommand, ProductReadDTO>
{
    public async Task<ProductReadDTO> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductByIdAsync(request.id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {request.id} not found.");
        }
        await _repository.DeleteProductByIdAsync(product.Id);
        return product;
    }
}
