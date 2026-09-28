using FluentValidation;
using Shop.Application.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Application.Validators.User
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("User email is required.")
                .EmailAddress().WithMessage("User email must be a valid email address.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("User password is required.")
                .MaximumLength(16).WithMessage("User password must not exceed 16 characters.")
                .MinimumLength(8).WithMessage("User password must be at least 8 characters long.");
        }
    }
}
