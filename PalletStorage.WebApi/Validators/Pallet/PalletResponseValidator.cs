using FluentValidation;
using PalletStorage.WebApi.Models.Pallet;

namespace PalletStorage.WebApi.Validators.Pallet;

public class PalletResponseValidator : AbstractValidator<PalletResponse>
{
    public PalletResponseValidator()
    {
        RuleFor(p => p.Id).NotNull().GreaterThan(0).WithMessage("Id must be > 0");
        RuleFor(p => p.Length).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
        RuleFor(p => p.Width).NotNull().GreaterThan(0).WithMessage("Width must be > 0");
        RuleFor(p => p.Height).NotNull().GreaterThan(0).WithMessage("Height must be > 0");
    }
}
