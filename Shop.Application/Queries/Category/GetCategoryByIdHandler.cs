using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Queries.Category;

public sealed class GetCategoryByIdHandler(ICategoryRepository repository)
    : IRequestHandler<GetCategoryByIdQuery, CategoryReadDTO?>
{
    public Task<CategoryReadDTO?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken) =>
        repository.GetCategoryByIdAsync(request.Id);
}
