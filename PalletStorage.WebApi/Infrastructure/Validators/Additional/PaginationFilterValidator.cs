using FluentValidation;
using PalletStorage.WebApi.Models.Additional;

namespace PalletStorage.WebApi.Infrastructure.Validators.Additional;

public class PaginationFilterValidator : AbstractValidator<PaginationFilter>
{
    public PaginationFilterValidator()
    {
        RuleFor(filter => filter.Take).NotNull().GreaterThan(0).WithMessage("Take parameter must be greater than 0!");
    }
}
