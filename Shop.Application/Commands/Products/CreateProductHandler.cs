using AutoMapper;
using MediatR;
using Shop.Api.Interface;
using Shop.Application.Interfaces.Repository;
using ShopDomain.Models;


namespace Shop.Application.Commands.Products;

public sealed class CreateProductHandler(
    IProductRepository repository,
    IImageService imageService)
    : IRequestHandler<CreateProductCommand, int?>
{
    public async Task<int?> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Product.Name,
            Description = request.Product.Description,
            Price = request.Product.Price,
            StockQty = request.Product.StockQty,
            CategoryId = request.Product.CategoryId
        };

        var productId = await repository.AddProductAsync(product);

        if (productId == null)
            return null;

        if (request.Image != null)
        {
            var fileName = await imageService.SaveProductImageAsync(
                request.Image
            );

            if (fileName != null)
            {
                await repository.AddProductImageAsync(
                    new ProductImage
                    {
                        ProductId = productId.Value,
                        FileName = fileName
                    }
                );
            }
        }

        return productId;
    }
}