﻿using FluentValidation;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.WebApi.Infrastructure.Validators.Box;

public class BoxUpdateRequestValidator : AbstractValidator<BoxUpdateRequest>
{
    public BoxUpdateRequestValidator()
    {
        RuleFor(b => b.Id).NotNull().GreaterThan(0).WithMessage("Id must be > 0");
        RuleFor(b => b.Length).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
        RuleFor(b => b.Width).NotNull().GreaterThan(0).WithMessage("Width must be > 0");
        RuleFor(b => b.Height).NotNull().GreaterThan(0).WithMessage("Height must be > 0");
        RuleFor(b => b.Weight).NotNull().GreaterThan(0).WithMessage("Weight must be > 0");
    }
}
