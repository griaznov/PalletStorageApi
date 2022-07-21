using FluentValidation;
using FluentValidation.Results;
using PalletStorage.WebApi.Models.Box;

namespace PalletStorage.WebApi.Validators.Box.RequestValidator;

public class BoxRequestValidator : IBoxRequestValidator
{
    private readonly IValidator<BoxCreateRequest> createValidator;
    private readonly IValidator<BoxUpdateRequest> updateValidator;

    public BoxRequestValidator(
        IValidator<BoxCreateRequest> createValidator,
        IValidator<BoxUpdateRequest> updateValidator)
    {
        this.createValidator = createValidator;
        this.updateValidator = updateValidator;
    }

    public async Task<ValidationResult> ValidateCreateAsync(BoxCreateRequest box)
    {
        var result = await createValidator.ValidateAsync(box);
        return result;
    }

    public async Task<ValidationResult> ValidateUpdateAsync(BoxUpdateRequest box)
    {
        var result = await updateValidator.ValidateAsync(box);
        return result;
    }
}
