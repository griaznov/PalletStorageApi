using FluentValidation;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Validators
{
    public class PalletValidator : AbstractValidator<PalletApiModel>
    {
        public PalletValidator()
        {
            RuleFor(p => p.Length).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
            RuleFor(p => p.Width).NotNull().GreaterThan(0).WithMessage("Width must be > 0");
            RuleFor(p => p.Height).NotNull().GreaterThan(0).WithMessage("Height must be > 0");
        }
    }
}
