using AutoMapper;
using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Queries.Category;

public class GetCategoryBySlugHandler(ICategoryRepository _repository, IMapper _mapper) : IRequestHandler<GetCategoryBySlugQuery, CategoryReadDTO>
{
    public async Task<CategoryReadDTO> Handle(GetCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetCategoryBySlugAsync(request.slug);
        return _mapper.Map<CategoryReadDTO>(category);
    }
}