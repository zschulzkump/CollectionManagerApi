using CollectionManagerApi.DTOs.Requests.CustomAttribute;
using FluentValidation;

namespace CollectionManagerApi.Validators.CustomAttribute;

public class UpdateCustomAttributeValidator : AbstractValidator<UpdateCustomAttributeRequest>
{
    public UpdateCustomAttributeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(255)
            .WithMessage("Name is required");
    }
}