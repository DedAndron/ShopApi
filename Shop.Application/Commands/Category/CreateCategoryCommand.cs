using MediatR;
using Shop.Application.DTOs.CategoryDTOs;

namespace Shop.Application.Commands.Category;

public sealed record CreateCategoryCommand(CategoryCreateDTO Category) : IRequest<int?>;