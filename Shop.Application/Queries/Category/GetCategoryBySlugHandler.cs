using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Queries.Category;

public sealed class GetCategoryBySlugHandler(ICategoryRepository repository)
    : IRequestHandler<GetCategoryBySlugQuery, CategoryReadDTO?>
{
    public Task<CategoryReadDTO?> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken) =>
        repository.GetCategoryBySlugAsync(request.Slug);
}