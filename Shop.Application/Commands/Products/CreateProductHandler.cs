using AutoMapper;
using MediatR;
using Shop.Application.Interfaces.Repository;
using ShopDomain.Models;
using Shop.Api.Interface;


namespace Shop.Application.Commands.Products;

public sealed class CreateProductHandler(IProductRepository _repository, IMapper _mapper, IImageService _imageService, IConfiguration _configuration)
    : IRequestHandler<CreateProductCommand, int?>
{
    public Task<int?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Product.Name,
            Description = request.Product.Description,
            Price = request.Product.Price,
            StockQty = request.Product.StockQty,
            CategoryId = request.Product.CategoryId
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);
        if (request.Image != null)
        {
            var fileName = await _imageService.SaveFileAsync(
                request.Image,
                _configuration["DirnameForFiles:Products"]!
            );

            if (fileName != null)
            {
                product.Images.Add(new ProductImage
                {
                    ProductId = product.Id,
                    FileName = fileName
                });

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}