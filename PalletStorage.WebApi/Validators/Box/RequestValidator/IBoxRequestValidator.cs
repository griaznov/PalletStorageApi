
using FluentValidation.Results;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.WebApi.Validators.Box.RequestValidator;

public interface IBoxRequestValidator
{
    public Task<ValidationResult> ValidateCreateAsync(BoxCreateRequest box);

    public Task<ValidationResult> ValidateUpdateAsync(BoxUpdateRequest box);
}
