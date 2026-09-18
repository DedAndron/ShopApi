using MediatR;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Repository;
using AutoMapper;

namespace Shop.Application.Commands.Product;

public class CreateProductHandler(IProductRepository _repository, IMapper _mapper) : IRequestHandler<CreateProductCommand, ProductReadDTO>
{
    public async Task<ProductReadDTO> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<ProductCreateDTO>(request);
        return await _repository.AddProductAsync(product);
    }
}