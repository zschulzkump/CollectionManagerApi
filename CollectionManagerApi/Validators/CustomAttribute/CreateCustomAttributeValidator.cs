using CollectionManagerApi.DTOs.Requests.CustomAttribute;
using FluentValidation;

namespace CollectionManagerApi.Validators.CustomAttribute;

public class CreateCustomAttributeValidator : AbstractValidator<CreateCustomAttributeRequest>
{
    public CreateCustomAttributeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(255)
            .WithMessage("Name is required");
    }
}