using AutoMapper;
using MediatR;
using Shop.Application.Interfaces.Repository;
using ShopDomain.Models;


namespace Shop.Application.Commands.Product;

public sealed class CreateProductHandler(IProductRepository _repository, IMapper _mapper)
    : IRequestHandler<CreateProductCommand, int?>
{
    public Task<int?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request.Product);
        return _repository.AddProductAsync(product);
    }
}