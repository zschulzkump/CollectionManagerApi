using CollectionManagerApi.DTOs.Requests.Tags;
using FluentValidation;

namespace CollectionManagerApi.Validators.Tag;

public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");
    }
}