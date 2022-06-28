using FluentValidation;
using PalletStorage.WebApi.Models.Models;

namespace PalletStorage.WebApi.Validators
{
    public class BoxValidator : AbstractValidator<BoxApiModel>
    {
        public BoxValidator()
        {
            RuleFor(p => p.Length).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
            RuleFor(p => p.Width).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
            RuleFor(p => p.Height).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
            RuleFor(b => b.Weight).NotNull().GreaterThan(0).WithMessage("Length must be > 0");
        }
    }
}
