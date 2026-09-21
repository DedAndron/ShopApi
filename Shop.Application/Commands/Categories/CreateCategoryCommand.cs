using MediatR;
using Shop.Application.DTOs.CategoryDTOs;

namespace Shop.Application.Commands.Categories;

public sealed record CreateCategoryCommand(CategoryCreateDTO Category) : IRequest<int?>;