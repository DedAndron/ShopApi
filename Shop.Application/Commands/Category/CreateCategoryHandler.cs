using AutoMapper;
using MediatR;
using Shop.Application.Interfaces.Repository;
using ShopDomain.Models;

namespace Shop.Application.Commands.Category;

public sealed class CreateCategoryHandler(ICategoryRepository repository, IMapper mapper)
    : IRequestHandler<CreateCategoryCommand, int?>
{
    public Task<int?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = mapper.Map<Category>(request.Category);
        return repository.AddCategoryAsync(category);
    }
}