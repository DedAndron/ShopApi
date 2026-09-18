using AutoMapper;
using MediatR;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Repository;
using Shop.Application.Queries.Category;

public class GetCategoryByIdHandler(ICategoryRepository _repository, IMapper _mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryReadDTO>
{
    public async Task<CategoryReadDTO> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetCategoryByIdAsync(request.id);
        return _mapper.Map<CategoryReadDTO>(category);
    }
}