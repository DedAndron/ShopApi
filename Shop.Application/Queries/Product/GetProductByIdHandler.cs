using AutoMapper;
using MediatR;
using Shop.Application.Interfaces.Repository;
using Shop.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Queries.Product;

public class GetProductByIdHandler (IProductRepository _repository, IMapper _mapper) : IRequestHandler<GetProductByIdQuery, ProductReadDTO>
{
    public async Task<ProductReadDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetProductByIdAsync(request.id);
        return _mapper.Map<ProductReadDTO>(product);
    }
}
