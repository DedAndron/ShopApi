using FluentValidation;
using Shop.Application.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;
namespace Shop.Application.Validators.Product;

public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
{
    public ProductCreateDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(10).WithMessage("Product name must not exceed 10 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(200).WithMessage("Product description must not exceed 200 characters.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than 0.");
        RuleFor(x => x.StockQty)
            .GreaterThanOrEqualTo(0).WithMessage("Product stock quantity must be greater than or equal to 0.");
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Product category ID must be greater than 0.");
    }
}
