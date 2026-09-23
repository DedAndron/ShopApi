using FluentValidation;
using Shop.Application.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Validators.Category;

public class CategoryCreateDTOValidator : AbstractValidator<CategoryCreateDTO>
{
    public CategoryCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
        RuleFor(x => x.Slug)
            .MaximumLength(100).WithMessage("Category slug must not exceed 100 characters.");
    }
}
